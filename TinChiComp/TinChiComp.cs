using System;
using System.Linq;
using QuanLyTinChi.Data;
using QuanLyTinChi.Models;

namespace QuanLyTinChi
{
    // Kế thừa MarshalByRefObject để cho phép truy cập từ xa (theo style CoTempConv)
    public class TinChiComp : MarshalByRefObject
    {
        private readonly AppDbContext _dbContext;

        public TinChiComp(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Phương thức kiểm tra điều kiện tín chỉ
        public bool KiemTraHanMuc(int tinChiHienTai, int tinChiThem)
        {
            const int MAX_TIN_CHI = 25;
            return (tinChiHienTai + tinChiThem) <= MAX_TIN_CHI;
        }

        // Phương thức xử lý đăng ký (Logic nghiệp vụ kết nối SQLite)
        public string DangKyHocPhan(string maSV, string maMon)
        {
            var section = _dbContext.Set<Section>().FirstOrDefault(s => s.SectionId == maMon);
            if (section == null)
            {
                return $"Lỗi: Không tìm thấy môn/lớp {maMon}.";
            }

            if (section.RegisteredCount >= section.MaxCapacity)
            {
                return $"Lỗi: Lớp tín chỉ {maMon} đã đầy.";
            }

            section.RegisteredCount++;
            _dbContext.SaveChanges();

            return string.Format("Ket qua: Sinh vien {0} da dang ky mon {1} thanh cong.", maSV, maMon);
        }

        // Tính học phí
        public double TinhHocPhi(int soTinChi)
        {
            double donGia = 500000.0;
            return (double)(soTinChi * donGia);
        }
    }
}
