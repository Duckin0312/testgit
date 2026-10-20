using System;
using EventTicket.Model.Enums;

namespace EventTicket.Model.Base
{
    public abstract class NguoiDungHeThong : NguoiDung
    {
        protected string maNguoiDung;
        protected string matKhau;
        protected QuyenHeThong quyen;

        protected NguoiDungHeThong() : base()
        {
        }

        // Constructor đầy đủ
        protected NguoiDungHeThong(string hoTen, string sdt, string email,
                                   string maNguoiDung, string matKhau, QuyenHeThong quyen)
            : base()
        {
            // Họ tên, sđt, email do NguoiDung kiểm tra và giữ giùm
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;

            // Còn mình thì tự giữ tài khoản, mật khẩu
            this.maNguoiDung = maNguoiDung;
            this.matKhau = matKhau;
            this.quyen = quyen;
        }

        // hàm dùng chung nhân viên và admin
        // kiểm tra mật khẩu
        public bool KiemTraMatKhau(string passNhapVao)
        {
            return matKhau != null && matKhau.Equals(passNhapVao);
        }

        // đổi mật khẩu
        public void DoiMatKhau(string passCu, string passMoi)
        {
            if (!KiemTraMatKhau(passCu))
            {
                throw new ArgumentException("Lỗi: Mật Khẩu cũ không chính xác!");
            }
            if (passMoi == null || passMoi.Length < 6)
            {
                throw new ArgumentException("Lỗi: Mật khẩu mới phải có ít nhất 6 ký tự! ");
            }
            matKhau = passMoi;
            Console.WriteLine("Đổi mật khẩu thành công");
        }

        public QuyenHeThong Quyen
        {
            get => quyen;
            set => quyen = value;
        }

        // hệ thống tự động tạo chưa thiết kế
        public string MaNguoiDung
        {
            get => maNguoiDung;
            set => maNguoiDung = value;
        }

        public string MatKhau
        {
            get => matKhau;
            set => matKhau = value;
        }
    }
}
