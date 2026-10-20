using System;
using System.Collections.Generic;

namespace EventTicket.Model.Base
{
    public abstract class NguoiDungBenNgoai : NguoiDung
    {
        protected DateTime? ngaySinh; // tạm thời chưa nghĩ ra gì hết
        protected string diaChi;

        protected NguoiDungBenNgoai() : base()
        {
        }

        protected NguoiDungBenNgoai(string hoTen, string sdt, string email, DateTime? ngaySinh, string diaChi)
            : base()
        {
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;
            NgaySinh = ngaySinh;
            this.diaChi = diaChi;
        }

        public abstract IEnumerable<object> GetLichSuGiaoDich();

        public DateTime? NgaySinh
        {
            get => ngaySinh;
            set
            {
                if (value != null && value.Value.Date < DateTime.Today)
                {
                    ngaySinh = value;
                }
                else
                {
                    throw new ArgumentException(" Ngày sinh không hợp lệ!");
                }
            }
        }

        public string DiaChi
        {
            get => diaChi;
            set => diaChi = value;
        }
    }
}
