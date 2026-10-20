using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Base;
using EventTicket.Model.Enums;
using EventTicket.Util;

namespace EventTicket.Model.Entity
{
    public class Admin : NguoiDungHeThong
    {
        public Admin() : base()
        {
            quyen = QuyenHeThong.ADMIN;
        }

        public Admin(string maNguoiDung, string matKhau, string hoTen, string sdt, string email) : base()
        {
            this.maNguoiDung = maNguoiDung;
            this.matKhau = matKhau;
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;
            quyen = QuyenHeThong.ADMIN;
        }

        public override string ThongTinLienHe()
        {
            return $"Admin: {hoTen} (Mã: {maNguoiDung}) - SĐT: {sdt} - Email: {email}";
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(maNguoiDung))
            {
                Console.WriteLine("Lỗi: Mã admin không được để trống!");
                return false;
            }
            if (matKhau == null || matKhau.Length < 6)
            {
                Console.WriteLine("Lỗi: Mật khẩu admin phải có ít nhất 6 ký tự!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                Console.WriteLine("Lỗi: Họ tên admin không được để trống!");
                return false;
            }
            return true;
        }

        public NhanVien TaoNhanVien(string maNguoiDung, string matKhau, string hoTen, string sdt, string email, string chucVu)
        {
            NhanVien nhanVien = new NhanVien(maNguoiDung, matKhau, hoTen, sdt, email, chucVu);
            if (!nhanVien.Validate())
            {
                throw new ArgumentException("Lỗi: Thông tin nhân viên không hợp lệ!");
            }
            return nhanVien;
        }

        public bool ResetMatKhau(NguoiDungHeThong nguoiDung, string matKhauMoi)
        {
            if (nguoiDung == null) return false;
            if (matKhauMoi == null || matKhauMoi.Length < 6) return false;
            nguoiDung.MatKhau = matKhauMoi;
            return true;
        }

        public bool CapQuyenNhanVien(NhanVien nhanVien, QuyenHeThong? quyenMoi)
        {
            if (nhanVien == null || quyenMoi == null) return false;
            nhanVien.Quyen = quyenMoi.Value;
            return true;
        }

        public bool KhoaTaiKhoan(NguoiDungHeThong nguoiDung)
        {
            if (nguoiDung == null) return false;
            nguoiDung.TrangThai = "INACTIVE";
            nguoiDung.NgayCapNhat = DateTime.Now;
            return true;
        }

        public bool MoKhoaTaiKhoan(NguoiDungHeThong nguoiDung)
        {
            if (nguoiDung == null) return false;
            nguoiDung.TrangThai = "ACTIVE";
            nguoiDung.NgayCapNhat = DateTime.Now;
            return true;
        }

        public long DemVeDaBan(IEnumerable<Ve> danhSachVe)
        {
            if (danhSachVe == null) return 0;
            return ThongKeUtils.Dem(danhSachVe, ve => ve != null && ve.TrangThaiVe == TrangThaiVe.SOLD);
        }

        public decimal TinhTongDoanhThuVeDaBan(IEnumerable<Ve> danhSachVe)
        {
            if (danhSachVe == null) return 0m;
            return ThongKeUtils.Tong(
                danhSachVe,
                ve => (ve != null && ve.TrangThaiVe == TrangThaiVe.SOLD) ? (ve.GiaVe ?? 0m) : 0m
            );
        }

        public Dictionary<string, decimal> ThongKeDoanhThuTheoSuKien(IEnumerable<Ve> danhSachVe)
        {
            if (danhSachVe == null) return new Dictionary<string, decimal>();
            return danhSachVe
                .Where(ve => ve != null)
                .Where(ve => ve.TrangThaiVe == TrangThaiVe.SOLD)
                .Where(ve => !string.IsNullOrWhiteSpace(ve.MaSuKien))
                .GroupBy(ve => ve.MaSuKien)
                .ToDictionary(g => g.Key, g => g.Aggregate(0m, (tong, ve) => tong + (ve.GiaVe ?? 0m)));
        }

        // Trả về null nếu không có sự kiện nào (thay cho Optional.empty() của Java)
        public string SuKienDoanhThuCaoNhat(IEnumerable<Ve> danhSachVe)
        {
            Dictionary<string, decimal> doanhThu = ThongKeDoanhThuTheoSuKien(danhSachVe);
            List<string> danhSachMaSuKien = ThongKeUtils.KhoaCoGiaTriLonNhat(doanhThu);
            return danhSachMaSuKien.FirstOrDefault();
        }

        public Dictionary<TrangThaiVe, long> ThongKeSoLuongVeTheoTrangThai(IEnumerable<Ve> danhSachVe)
        {
            if (danhSachVe == null) return new Dictionary<TrangThaiVe, long>();
            return danhSachVe
                .Where(ve => ve != null)
                .Where(ve => ve.TrangThaiVe != null)
                .GroupBy(ve => ve.TrangThaiVe.Value)
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }

        public Dictionary<string, List<Ve>> ThongKeVeTheoKhachHang(IEnumerable<Ve> danhSachVe)
        {
            if (danhSachVe == null) return new Dictionary<string, List<Ve>>();
            return danhSachVe
                .Where(ve => ve != null)
                .Where(ve => !string.IsNullOrWhiteSpace(ve.MaKhachHang))
                .GroupBy(ve => ve.MaKhachHang)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public bool KiemTraQuyenQuanTri()
        {
            return quyen == QuyenHeThong.ADMIN;
        }
    }
}
