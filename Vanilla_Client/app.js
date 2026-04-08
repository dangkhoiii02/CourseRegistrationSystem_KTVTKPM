const API = '';

// ===== State =====
let allSections = [];
let myEnrollments = [];

// ===== Init =====
document.addEventListener('DOMContentLoaded', () => {
    loadSections();
});

// ===== Tab Switching =====
function switchTab(tabName) {
    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active'));
    document.querySelector(`[data-tab="${tabName}"]`).classList.add('active');
    document.getElementById(`tab-${tabName}`).classList.add('active');

    if (tabName === 'enrollment') {
        loadAvailableSections();
    } else {
        loadSections();
    }
}

// ===== API Helpers =====
async function apiGet(url) {
    const res = await fetch(API + url);
    if (!res.ok) {
        const err = await res.json().catch(() => ({}));
        throw new Error(err.message || 'Lỗi server');
    }
    return res.json();
}

async function apiPost(url, body) {
    const res = await fetch(API + url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    });
    const data = await res.json().catch(() => ({}));
    if (!res.ok) throw new Error(data.message || data.title || JSON.stringify(data.errors || data) || 'Lỗi server');
    return data;
}

async function apiPut(url, body) {
    const res = await fetch(API + url, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    });
    const data = await res.json().catch(() => ({}));
    if (!res.ok) throw new Error(data.message || data.title || JSON.stringify(data.errors || data) || 'Lỗi server');
    return data;
}

async function apiDelete(url) {
    const res = await fetch(API + url, { method: 'DELETE' });
    const data = await res.json().catch(() => ({}));
    if (!res.ok) throw new Error(data.message || data.title || JSON.stringify(data.errors || data) || 'Lỗi server');
    return data;
}

// ===== Load Sections =====
async function loadSections() {
    try {
        allSections = await apiGet('/api/sections');
        renderSectionsTable();
        populateSectionDropdown();
        renderAvailableSections();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

function populateSectionDropdown() {
    const sel = document.getElementById('reg-section');
    if(!sel) return;
    sel.innerHTML = '<option value="">-- Chọn lớp --</option>';
    allSections.forEach(s => {
        sel.innerHTML += `<option value="${s.sectionId}">${s.sectionId} — ${s.subjectName} (${s.registeredCount}/${s.maxCapacity})</option>`;
    });
}

function getCapacityColor(count, max) {
    const pct = (count / max) * 100;
    if (pct >= 90) return '#ef4444';
    if (pct >= 60) return '#f59e0b';
    return '#10b981';
}

// ===== Render Sections Table (Tab 2) =====
function renderSectionsTable() {
    const container = document.getElementById('sections-table');
    if (!allSections.length) {
        container.innerHTML = `<div class="empty-state"><p>Chưa có lớp tín chỉ nào</p></div>`;
        return;
    }

    let html = `<div style="overflow-x:auto"><table class="data-table">
        <thead><tr>
            <th>Mã lớp</th>
            <th>Môn học</th>
            <th>TC</th>
            <th>Giảng viên</th>
            <th>Nhóm</th>
            <th>Sĩ số</th>
            <th>Lịch học</th>
            <th>Học kỳ</th>
            <th>Trạng thái</th>
        </tr></thead><tbody>`;

    allSections.forEach(s => {
        const pct = s.maxCapacity > 0 ? (s.registeredCount / s.maxCapacity) * 100 : 0;
        const color = getCapacityColor(s.registeredCount, s.maxCapacity);

        html += `<tr class="clickable-row" onclick="editSection('${s.sectionId}')">
            <td><strong>${s.sectionId}</strong></td>
            <td>${s.subjectName}<br><small style="color:var(--text-dim)">${s.subjectId}</small></td>
            <td>${s.credits}</td>
            <td>${s.lecturerName}</td>
            <td>${s.groupNumber}</td>
            <td>
                <div class="capacity-bar">
                    <div class="capacity-track"><div class="capacity-fill" style="width:${pct}%;background:${color}"></div></div>
                    <span class="capacity-text">${s.registeredCount}/${s.maxCapacity}</span>
                </div>
            </td>
            <td>${s.scheduleInfo}</td>
            <td>${s.semesterName}</td>
            <td>${s.isActive ? '<span class="badge badge-success">Mở</span>' : '<span class="badge badge-danger">Khóa</span>'}</td>
        </tr>`;
    });

    html += '</tbody></table></div>';
    container.innerHTML = html;
}

// ===== Render Available Sections (Tab 1) =====
function renderAvailableSections() {
    const container = document.getElementById('available-sections');
    if (!container) return; // if hiding tab 1
    if (!allSections.length) {
        container.innerHTML = `<div class="empty-state"><p>Không có lớp nào đang mở</p></div>`;
        return;
    }

    let html = `<div style="overflow-x:auto"><table class="data-table">
        <thead><tr>
            <th>Mã lớp</th>
            <th>Môn học</th>
            <th>TC</th>
            <th>Giảng viên</th>
            <th>Học kỳ</th>
            <th>Lịch học</th>
            <th>Sĩ số</th>
            <th>Đăng ký</th>
        </tr></thead><tbody>`;

    allSections.forEach(s => {
        const pct = s.maxCapacity > 0 ? (s.registeredCount / s.maxCapacity) * 100 : 0;
        const color = getCapacityColor(s.registeredCount, s.maxCapacity);
        const isFull = s.registeredCount >= s.maxCapacity;

        const isRegistered = myEnrollments.includes(s.sectionId);

        let btnText = 'Đăng ký';
        let disabledAttr = '';
        if (isRegistered) {
            btnText = 'Đã ĐK';
            disabledAttr = 'disabled style="opacity:0.6"';
        } else if (isFull) {
            btnText = 'Đầy';
            disabledAttr = 'disabled style="opacity:0.5"';
        }

        const btnHtml = `<button class="btn btn-primary btn-xs" onclick="quickRegister('${s.sectionId}')" ${disabledAttr}>
                ${btnText}
               </button>`;

        html += `<tr>
            <td><strong>${s.sectionId}</strong></td>
            <td>${s.subjectName}</td>
            <td>${s.credits}</td>
            <td>${s.lecturerName}</td>
            <td>${s.semesterName}</td>
            <td>${s.scheduleInfo}</td>
            <td>
                <div class="capacity-bar">
                    <div class="capacity-track"><div class="capacity-fill" style="width:${pct}%;background:${color}"></div></div>
                    <span class="capacity-text">${s.registeredCount}/${s.maxCapacity}</span>
                </div>
            </td>
            <td>${btnHtml}</td>
        </tr>`;
    });

    html += '</tbody></table></div>';
    container.innerHTML = html;
}

function loadAvailableSections() {
    renderAvailableSections();
}

function getMyEnrollments() {
     const stored = localStorage.getItem('local_enrollments');
     return stored ? JSON.parse(stored) : [];
}

function saveMyEnrollments(arr) {
     localStorage.setItem('local_enrollments', JSON.stringify(arr));
}

function loadMyEnrollments() {
     myEnrollments = getMyEnrollments();
     const container = document.getElementById('my-enrollments');
     if(!container) return;
     
     if (myEnrollments.length === 0) {
         container.innerHTML = `<div class="empty-state"><p style="color:var(--text-dim)">Bạn chưa đăng ký môn nào</p></div>`;
         return;
     }

     let html = `<div style="overflow-x:auto"><table class="data-table">
        <thead><tr>
            <th>Mã lớp</th>
            <th>Môn học</th>
            <th>TC</th>
            <th>Giảng viên</th>
            <th>Lịch học</th>
            <th>Trạng thái</th>
            <th>Thao tác</th>
        </tr></thead><tbody>`;

     myEnrollments.forEach(id => {
         const s = allSections.find(x => x.sectionId === id);
         if (!s) return; // section probably deleted

         html += `<tr>
            <td><strong>${s.sectionId}</strong></td>
            <td>${s.subjectName}</td>
            <td>${s.credits}</td>
            <td>${s.lecturerName}</td>
            <td>${s.scheduleInfo}</td>
            <td><span class="badge badge-success">Đã đăng ký</span></td>
            <td><button class="btn btn-danger btn-xs" onclick="cancelEnrollment('${s.sectionId}')">Huỷ ĐK</button></td>
         </tr>`;
     });

     html += '</tbody></table></div>';
     container.innerHTML = html;
}

async function quickRegister(sectionId) {
    if (myEnrollments.includes(sectionId)) return;
    
    const section = allSections.find(s => s.sectionId === sectionId);
    if (!section) return;

    const maSV = "SV_DEFAULT"; // Không cần nhập mã sinh viên nữa

    try {
        // Gọi đến API của TinChiComp wrapper mới
        const res = await apiPost(`/api/tinchi/dangky?maSV=${encodeURIComponent(maSV)}&maMon=${encodeURIComponent(sectionId)}`, {});
        
        // Vẫn cập nhật số lượng trên SectionsController cũ để UI đầy đủ chức năng
        section.registeredCount += 1;
        await apiPut(`/api/sections/${sectionId}`, section);
        
        myEnrollments.push(sectionId);
        saveMyEnrollments(myEnrollments);
        
        showToast(res.message || 'Đăng ký lớp thành công!', 'success');
        loadSections();
        loadMyEnrollments();
    } catch(e) {
        showToast(e.message, 'error');
    }
}

async function cancelEnrollment(sectionId) {
    if (!confirm('Bạn chắc chắn muốn huỷ đăng ký lớp này?')) return;

    const section = allSections.find(s => s.sectionId === sectionId);
    if (!section) {
        // Just remove locally
        myEnrollments = myEnrollments.filter(id => id !== sectionId);
        saveMyEnrollments(myEnrollments);
        loadMyEnrollments();
        return;
    }

    try {
        section.registeredCount = Math.max(0, section.registeredCount - 1);
        await apiPut(`/api/sections/${sectionId}`, section);

        myEnrollments = myEnrollments.filter(id => id !== sectionId);
        saveMyEnrollments(myEnrollments);
        
        showToast('Đã huỷ đăng ký lớp!', 'success');
        loadSections();
        loadMyEnrollments();
    } catch(e) {
        showToast(e.message, 'error');
    }
}

function registerEnrollment() {
    const sectionId = document.getElementById('reg-section').value;
    if(!sectionId) {
        showToast('Vui lòng chọn 1 lớp', 'error');
        return;
    }
    quickRegister(sectionId);
}

// ===== Section CRUD (Tab 2) =====
async function handleSectionSubmit(event) {
    event.preventDefault();
    const mode = document.getElementById('edit-mode').value;

    const sectionId = document.getElementById('f-sectionId').value;
    const subjectId = document.getElementById('f-subjectId').value;
    const subjectName = document.getElementById('f-subjectName').value;
    const semesterName = document.getElementById('f-semesterName').value;
    const credits = parseInt(document.getElementById('f-credits').value) || 3;
    const groupNumber = parseInt(document.getElementById('f-groupNumber').value) || 1;
    const lecturerName = document.getElementById('f-lecturerName').value;
    const maxCapacity = parseInt(document.getElementById('f-maxCapacity').value) || 80;
    const scheduleInfo = document.getElementById('f-scheduleInfo').value;

    if (!sectionId) {
        showToast('Mã lớp tín chỉ không được bỏ trống.', 'error');
        return;
    }

    try {
        if (mode === 'create') {
            await apiPost('/api/sections', { sectionId, subjectId, subjectName, semesterName, credits, groupNumber, lecturerName, maxCapacity, scheduleInfo });
            showToast('Thêm lớp tín chỉ thành công!', 'success');
            resetForm();
        } else if (mode === 'edit') {
            await apiPut(`/api/sections/${sectionId}`, { subjectId, subjectName, semesterName, credits, groupNumber, lecturerName, maxCapacity, scheduleInfo });
            showToast('Cập nhật thành công!', 'success');
        }
        loadSections();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

function editSection(sectionId) {
    const section = allSections.find(s => s.sectionId === sectionId);
    if (!section) return;

    document.getElementById('edit-mode').value = 'edit';
    document.getElementById('f-sectionId').value = section.sectionId;
    document.getElementById('f-sectionId').disabled = true; // Disable ID modification
    document.getElementById('f-subjectId').value = section.subjectId;
    document.getElementById('f-subjectName').value = section.subjectName;
    document.getElementById('f-semesterName').value = section.semesterName;
    document.getElementById('f-credits').value = section.credits;
    document.getElementById('f-groupNumber').value = section.groupNumber;
    document.getElementById('f-lecturerName').value = section.lecturerName;
    document.getElementById('f-maxCapacity').value = section.maxCapacity;
    document.getElementById('f-scheduleInfo').value = section.scheduleInfo;

    // Scroll to form automatically
    document.querySelector('.form-card').scrollIntoView({ behavior: 'smooth' });
}

async function deleteFromForm() {
    const mode = document.getElementById('edit-mode').value;
    const sectionId = document.getElementById('f-sectionId').value;
    
    if (mode === 'create' || !sectionId) {
        showToast('Vui lòng chọn một lớp cụ thể ở danh sách để xóa!', 'error');
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn xóa lớp ${sectionId}?`)) return;

    try {
        const result = await apiDelete(`/api/sections/${sectionId}`);
        showToast(result.message || 'Xóa lớp tín chỉ thành công', 'success');
        resetForm();
        loadSections();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

function resetForm() {
    document.getElementById('section-form').reset();
    document.getElementById('edit-mode').value = 'create';
    document.getElementById('f-sectionId').disabled = false;
    document.getElementById('f-groupNumber').value = '1';
    document.getElementById('f-maxCapacity').value = '80';
    document.getElementById('f-credits').value = '3';
}

// ===== Toast =====
function showToast(message, type = 'info') {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.className = `toast ${type} show`;
    setTimeout(() => {
        toast.classList.remove('show');
    }, 3500);
}
