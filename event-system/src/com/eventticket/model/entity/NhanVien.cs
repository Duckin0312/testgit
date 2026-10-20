using System;
using EventTicket.Model.Base;
using EventTicket.Model.Enums;

namespace EventTicket.Model.Entity
{
    public class NhanVien : NguoiDungHeThong
    {
        public string ChucVu { get; set; } // "ke toan", "hoi tro khach" ...
        public decimal? LuongCoBan { get; set; }

        public NhanVien() : base() // gọi constructor của lớp cha
        {
        }

        public NhanVien(string maNguoiDung, string matKhau, string hoTen, string sdt, string email, string chucVu) : base()
        {
            this.maNguoiDung = maNguoiDung;
            this.matKhau = matKhau;
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;
            quyen = QuyenHeThong.NHAN_VIEN;
            ChucVu = chucVu;
        }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(maNguoiDung)) { return false; }
            if (matKhau == null || matKhau.Length < 6) // đặt số lượng mật khẩu tùy chỉnh sau nha
            {
                Console.WriteLine("Lỗi: Mật khẩu phải có ít nhất 6 ký tự!");
                return false;
            }
            if (string.IsNullOrEmpty(hoTen)) { return false; }

            return true;
        }

        public override string ThongTinLienHe()
        {
            return $"Nhân viên: {hoTen} (Mã: {maNguoiDung}) - Chức vụ: {ChucVu} - SĐT Nội bộ: {sdt}";
        }

        // hàm riêng cho class nhân viên
        public decimal TinhLuong(int soNgayLam)
        {
            if (LuongCoBan == null)
            {
                return 0m;
            }
            // HALF_UP của Java tương đương MidpointRounding.AwayFromZero
            return Math.Round(LuongCoBan.Value / 22m, 2, MidpointRounding.AwayFromZero) * soNgayLam;
        }
    }
}
