using System;
using System.Collections.Generic;
using EventTicket.Model.Base;
using LoaiKhachHangEnum = EventTicket.Model.Enums.LoaiKhachHang;

namespace EventTicket.Model.Entity
{
    public class KhachHang : NguoiDungBenNgoai
    {
        private readonly List<Ve> lichSuMuaVe = new List<Ve>();

        public LoaiKhachHangEnum? LoaiKhachHang { get; set; } = LoaiKhachHangEnum.THUONG;

        public KhachHang() : base()
        {
        }

        public KhachHang(string maKhachHang, string hoTen, string sdt, string email,
                         DateTime ngaySinh, string diaChi, LoaiKhachHangEnum? loaiKhachHang) : base()
        {
            Id = maKhachHang;
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;
            NgaySinh = ngaySinh;
            this.diaChi = diaChi;
            LoaiKhachHang = loaiKhachHang;
        }

        public string MaKhachHang
        {
            get => Id;
            set => Id = value;
        }

        public bool MuaVe(Ve ve)
        {
            if (ve == null || !ve.Validate()) return false;
            if (!ve.MaKhachHang.Equals(Id)) return false;
            lichSuMuaVe.Add(ve);
            return true;
        }

        public override IEnumerable<object> GetLichSuGiaoDich()
        {
            return new List<Ve>(lichSuMuaVe);
        }

        // Bản có kiểu cụ thể (List<Ve>) cho tiện sử dụng
        public List<Ve> GetLichSuMuaVe()
        {
            return new List<Ve>(lichSuMuaVe);
        }

        public override string ThongTinLienHe()
        {
            return hoTen + " - " + sdt + " - " + email;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Console.WriteLine("Lỗi: ID không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(hoTen))
            {
                Console.WriteLine("Lỗi: Họ tên không được để trống!");
                return false;
            }
            else if (LoaiKhachHang == null)
            {
                Console.WriteLine("Lỗi: Loại khách hàng không được để trống!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return Id + " (" + hoTen + ", " + LoaiKhachHang + ")";
        }
    }
}
