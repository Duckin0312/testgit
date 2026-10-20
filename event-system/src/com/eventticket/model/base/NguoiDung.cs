using System;
using System.Text.RegularExpressions;

namespace EventTicket.Model.Base
{
    public abstract class NguoiDung : BaseEntity
    {
        protected string hoTen;
        protected string sdt;
        protected string email;

        public abstract string ThongTinLienHe();

        public string HoTen
        {
            get => hoTen;
            set
            {
                // Regex giải thích:
                // \p{L} : Cho phép nhập TẤT CẢ các chữ cái trên thế giới (kể cả chữ có dấu tiếng Việt như á, ố, ươ...)
                // Khoảng trắng " " : Cho phép nhập dấu cách giữa các chữ
                // {2,50} : Độ dài tên bắt buộc từ 2 đến 50 ký tự
                if (value != null && Regex.IsMatch(value.Trim(), @"^[\p{L} ]{2,50}\z"))
                {
                    hoTen = value.Trim();
                }
                else
                {
                    throw new ArgumentException("lỗi: tên không hợp lệ");
                }
            }
        }

        public string Sdt
        {
            get => sdt;
            set
            {
                if (value != null && Regex.IsMatch(value, @"^0[0-9]{9}\z")) // bắt đầu bằng 0, theo sau là 9 chữ số
                {
                    sdt = value;
                }
                else
                {
                    throw new ArgumentException("Lỗi: Số ddienj thoại phải bắt đầu bằng 0 và có 10 chữ số!");
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                // regex kiểm tra chữ cái A-Z a-z 0-9 + @ + (không bao gồm ký tự đặc biệt và khoảng trắng)
                if (value != null && Regex.IsMatch(value, @"^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\z"))
                {
                    email = value;
                }
                else
                {
                    throw new ArgumentException("Lỗi: Định dạng email không hợp lệ ");
                }
            }
        }
    }
}
