const API = '';

// ===== State =====
let allSections = [];
let lookupSubjects = [];
let lookupLecturers = [];
let lookupSemesters = [];

// ===== Init =====
document.addEventListener('DOMContentLoaded', () => {
    loadLookups();
    loadSections();
    loadMyEnrollments();
});

// ===== Load Lookups =====
async function loadLookups() {
    try {
        const [subjects, lecturers, semesters] = await Promise.all([
            apiGet('/api/lookup/subjects'),
            apiGet('/api/lookup/lecturers'),
            apiGet('/api/lookup/semesters')
        ]);
        lookupSubjects = subjects;
        lookupLecturers = lecturers;
        lookupSemesters = semesters;
        populateFormDropdowns();
    } catch (e) {
        console.error('Failed to load lookups:', e);
    }
}

function populateFormDropdowns() {
    const subjectSel = document.getElementById('f-subjectId');
    subjectSel.innerHTML = '<option value="">-- Chọn môn --</option>';
    lookupSubjects.forEach(s => {
        subjectSel.innerHTML += `<option value="${s.subjectId}">${s.subjectId} — ${s.subjectName} (${s.credits} TC)</option>`;
    });

    const lecturerSel = document.getElementById('f-lecturerId');
    lecturerSel.innerHTML = '<option value="">-- Chọn GV --</option>';
    lookupLecturers.forEach(l => {
        lecturerSel.innerHTML += `<option value="${l.lecturerId}">${l.lecturerId} — ${l.fullName}</option>`;
    });

    const semesterSel = document.getElementById('f-semesterId');
    semesterSel.innerHTML = '<option value="">-- Chọn HK --</option>';
    lookupSemesters.forEach(s => {
        semesterSel.innerHTML += `<option value="${s.semesterId}">${s.semesterName}</option>`;
    });
    // Auto-select first semester
    if (lookupSemesters.length > 0) {
        semesterSel.value = lookupSemesters[0].semesterId;
    }
}

// ===== Tab Switching =====
function switchTab(tabName) {
    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active'));
    document.querySelector(`[data-tab="${tabName}"]`).classList.add('active');
    document.getElementById(`tab-${tabName}`).classList.add('active');

    if (tabName === 'enrollment') {
        loadMyEnrollments();
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
    sel.innerHTML = '<option value="">-- Chọn lớp --</option>';
    allSections.forEach(s => {
        sel.innerHTML += `<option value="${s.sectionId}">${s.sectionId} — ${s.subjectName} (${s.registeredCount}/${s.maxCapacity})</option>`;
    });
}

// Helper functions
function getDayName(d) {
    const days = { 2: 'Thứ 2', 3: 'Thứ 3', 4: 'Thứ 4', 5: 'Thứ 5', 6: 'Thứ 6', 7: 'Thứ 7', 8: 'CN' };
    return days[d] || `Ngày ${d}`;
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
        container.innerHTML = `<div class="empty-state"><div class="emoji">📭</div><p>Chưa có lớp tín chỉ nào</p></div>`;
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
            <th>Trạng thái</th>
            <th>Thao tác</th>
        </tr></thead><tbody>`;

    allSections.forEach(s => {
        const scheduleHtml = s.schedules?.map(sch =>
            `<span class="schedule-info">${getDayName(sch.dayOfWeek)}, tiết ${sch.startPeriod}-${sch.startPeriod + sch.periodCount - 1}, ${sch.room}</span>`
        ).join(' ') || '<span style="color:var(--text-dim)">—</span>';

        const pct = s.maxCapacity > 0 ? (s.registeredCount / s.maxCapacity) * 100 : 0;
        const color = getCapacityColor(s.registeredCount, s.maxCapacity);

        html += `<tr>
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
            <td>${scheduleHtml}</td>
            <td>${s.isActive ? '<span class="badge badge-success">Mở</span>' : '<span class="badge badge-danger">Khóa</span>'}</td>
            <td>
                <div class="action-btns">
                    <button class="btn btn-warning btn-xs" onclick="editSection('${s.sectionId}')">✏️</button>
                    <button class="btn btn-danger btn-xs" onclick="deleteSection('${s.sectionId}')">🗑️</button>
                </div>
            </td>
        </tr>`;
    });

    html += '</tbody></table></div>';
    container.innerHTML = html;
}

// ===== Render Available Sections (Tab 1) =====
function renderAvailableSections() {
    const container = document.getElementById('available-sections');
    if (!allSections.length) {
        container.innerHTML = `<div class="empty-state"><div class="emoji">📭</div><p>Không có lớp nào đang mở</p></div>`;
        return;
    }

    let html = `<div style="overflow-x:auto"><table class="data-table">
        <thead><tr>
            <th>Mã lớp</th>
            <th>Môn học</th>
            <th>TC</th>
            <th>Giảng viên</th>
            <th>Lịch học</th>
            <th>Sĩ số</th>
            <th>Đăng ký</th>
        </tr></thead><tbody>`;

    allSections.forEach(s => {
        const scheduleHtml = s.schedules?.map(sch =>
            `<span class="schedule-info">${getDayName(sch.dayOfWeek)}, tiết ${sch.startPeriod}-${sch.startPeriod + sch.periodCount - 1}, ${sch.room}</span>`
        ).join(' ') || '—';

        const pct = s.maxCapacity > 0 ? (s.registeredCount / s.maxCapacity) * 100 : 0;
        const color = getCapacityColor(s.registeredCount, s.maxCapacity);
        const isFull = s.registeredCount >= s.maxCapacity;
        const isRegistered = myEnrollments.some(e => e.sectionId === s.sectionId && e.status === 'Active');

        // Grey out if registered
        const rowStyle = isRegistered ? 'opacity: 0.6; background-color: var(--card-bg-light);' : '';
        const btnHtml = isRegistered 
            ? `<button class="btn btn-secondary btn-xs" disabled>✔️ Đã ĐK</button>`
            : `<button class="btn btn-primary btn-xs" onclick="quickRegister('${s.sectionId}')" ${isFull ? 'disabled style="opacity:0.5"' : ''}>
                ${isFull ? '🔒 Đầy' : '✅ Đăng ký'}
               </button>`;

        html += `<tr style="${rowStyle}">
            <td><strong>${s.sectionId}</strong></td>
            <td>${s.subjectName}</td>
            <td>${s.credits}</td>
            <td>${s.lecturerName}</td>
            <td>${scheduleHtml}</td>
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

let myEnrollments = [];

// ===== Load My Enrollments =====
async function loadMyEnrollments() {
    const studentId = document.getElementById('reg-student').value || '1';
    const container = document.getElementById('my-enrollments');

    try {
        const enrollments = await apiGet(`/api/enrollments/student/${studentId}`);
        myEnrollments = enrollments;

        renderAvailableSections(); // Re-render available sections to update dim states

        if (!enrollments.length) {
            container.innerHTML = `<div class="empty-state"><div class="emoji">📋</div><p>Chưa đăng ký lớp nào</p></div>`;
            return;
        }

        let html = `<table class="data-table">
            <thead><tr>
                <th>Mã lớp</th>
                <th>Môn học</th>
                <th>Học kỳ</th>
                <th>Ngày ĐK</th>
                <th>Trạng thái</th>
                <th>Thao tác</th>
            </tr></thead><tbody>`;

        enrollments.forEach(e => {
            html += `<tr>
                <td><strong>${e.sectionId}</strong></td>
                <td>${e.subjectName}<br><small style="color:var(--text-dim)">${e.subjectId}</small></td>
                <td>${e.semesterId}</td>
                <td>${new Date(e.enrolledAt).toLocaleDateString('vi-VN')}</td>
                <td><span class="badge badge-success">✅ ${e.status}</span></td>
                <td>
                    <button class="btn btn-danger btn-xs" onclick="cancelEnrollment('${e.studentId}','${e.sectionId}')">❌ Hủy</button>
                </td>
            </tr>`;
        });

        html += '</tbody></table>';
        container.innerHTML = html;
    } catch (e) {
        container.innerHTML = `<div class="empty-state"><p style="color:var(--danger)">⚠️ ${e.message}</p></div>`;
    }
}

// ===== Enrollment Actions =====
async function registerEnrollment() {
    const studentId = document.getElementById('reg-student').value;
    const sectionId = document.getElementById('reg-section').value;

    if (!studentId || !sectionId) {
        showToast('Vui lòng chọn đầy đủ thông tin', 'error');
        return;
    }

    try {
        const result = await apiPost('/api/enrollments/register', { studentId, sectionId });
        showToast(result.message, 'success');
        loadSections();
        loadMyEnrollments();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

async function quickRegister(sectionId) {
    const studentId = document.getElementById('reg-student').value || '1';
    try {
        const result = await apiPost('/api/enrollments/register', { studentId, sectionId });
        showToast(result.message, 'success');
        loadSections();
        loadMyEnrollments();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

async function cancelEnrollment(studentId, sectionId) {
    if (!confirm('Bạn chắc chắn muốn hủy đăng ký?')) return;

    try {
        const result = await apiPost('/api/enrollments/cancel', { studentId, sectionId });
        showToast(result.message, 'success');
        loadSections();
        loadMyEnrollments();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

// ===== Section CRUD (Tab 2) =====
async function handleSectionSubmit(event) {
    event.preventDefault();
    const mode = document.getElementById('edit-mode').value;

    const sectionId = document.getElementById('f-sectionId').value;
    const subjectId = document.getElementById('f-subjectId').value;
    const semesterId = document.getElementById('f-semesterId').value;
    const groupNumber = parseInt(document.getElementById('f-groupNumber').value) || 1;
    const lecturerId = document.getElementById('f-lecturerId').value;
    const maxCapacity = parseInt(document.getElementById('f-maxCapacity').value) || 80;

    const schedules = [{
        dayOfWeek: parseInt(document.getElementById('f-dayOfWeek').value) || 2,
        startPeriod: parseInt(document.getElementById('f-startPeriod').value) || 1,
        periodCount: parseInt(document.getElementById('f-periodCount').value) || 3,
        room: document.getElementById('f-room').value || ""
    }];

    try {
        if (mode === 'create') {
            await apiPost('/api/sections', { sectionId, subjectId, semesterId, groupNumber, lecturerId, maxCapacity, schedules });
            showToast('Thêm lớp tín chỉ thành công!', 'success');
        } else {
            await apiPut(`/api/sections/${sectionId}`, { subjectId, lecturerId, maxCapacity, schedules });
            showToast('Cập nhật thành công!', 'success');
        }
        resetForm();
        loadSections();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

function editSection(sectionId) {
    const section = allSections.find(s => s.sectionId === sectionId);
    if (!section) return;

    document.getElementById('edit-mode').value = 'edit';
    document.getElementById('form-title').textContent = '✏️ Sửa lớp tín chỉ';
    document.getElementById('f-sectionId').value = section.sectionId;
    document.getElementById('f-sectionId').disabled = true;
    document.getElementById('f-subjectId').value = section.subjectId;
    document.getElementById('f-subjectId').disabled = false;
    document.getElementById('f-semesterId').value = section.semesterId;
    document.getElementById('f-semesterId').disabled = true;
    document.getElementById('f-groupNumber').value = section.groupNumber;
    document.getElementById('f-lecturerId').value = section.lecturerId;
    document.getElementById('f-maxCapacity').value = section.maxCapacity;

    document.getElementById('f-dayOfWeek').disabled = false;
    document.getElementById('f-startPeriod').disabled = false;
    document.getElementById('f-periodCount').disabled = false;
    document.getElementById('f-room').disabled = false;

    if (section.schedules && section.schedules.length > 0) {
        const sch = section.schedules[0];
        document.getElementById('f-dayOfWeek').value = sch.dayOfWeek;
        document.getElementById('f-startPeriod').value = sch.startPeriod;
        document.getElementById('f-periodCount').value = sch.periodCount;
        document.getElementById('f-room').value = sch.room;
    } else {
        document.getElementById('f-dayOfWeek').value = '2';
        document.getElementById('f-startPeriod').value = '1';
        document.getElementById('f-periodCount').value = '3';
        document.getElementById('f-room').value = '';
    }

    // Scroll to form
    document.querySelector('.form-card').scrollIntoView({ behavior: 'smooth' });
}

async function deleteSection(sectionId) {
    if (!confirm(`Bạn chắc chắn muốn xóa lớp ${sectionId}?`)) return;

    try {
        const result = await apiDelete(`/api/sections/${sectionId}`);
        showToast(result.message, 'success');
        loadSections();
    } catch (e) {
        showToast(e.message, 'error');
    }
}

function resetForm() {
    document.getElementById('section-form').reset();
    document.getElementById('edit-mode').value = 'create';
    document.getElementById('form-title').textContent = '➕ Thêm lớp tín chỉ mới';
    document.getElementById('f-sectionId').disabled = false;
    document.getElementById('f-subjectId').disabled = false;
    document.getElementById('f-semesterId').disabled = false;
    
    document.getElementById('f-dayOfWeek').disabled = false;
    document.getElementById('f-startPeriod').disabled = false;
    document.getElementById('f-periodCount').disabled = false;
    document.getElementById('f-room').disabled = false;

    document.getElementById('f-groupNumber').value = '1';
    document.getElementById('f-maxCapacity').value = '80';
    // Auto-select first semester
    if (lookupSemesters.length > 0) {
        document.getElementById('f-semesterId').value = lookupSemesters[0].semesterId;
    }
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
