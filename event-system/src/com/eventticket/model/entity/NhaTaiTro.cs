using System;
using System.Collections.Generic;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class NhaTaiTro : NguoiDungBenNgoai
    {
        public string LinhVucHoatDong { get; set; } // nước ngọt, đồ ăn nhanh, chăm sóc sức khỏe ....
        public string NguoiDaiDien { get; set; }

        // constructor rỗng
        public NhaTaiTro() : base()
        {
        }

        // constructor đầy đủ
        public NhaTaiTro(string hoTen, string sdt, string email, string linhVucHoatDong, string nguoiDaiDien) : base()
        {
            HoTen = hoTen;
            Sdt = sdt;
            Email = email;
            NguoiDaiDien = nguoiDaiDien;
            LinhVucHoatDong = linhVucHoatDong;
        }

        // ghi đè hàm của NguoiDungBenNgoai
        public override IEnumerable<object> GetLichSuGiaoDich()
        {
            // trả về danh sách TaiTro để xem họ đã tài trợ bao nhiêu sự kiện
            return new List<object>();
        }

        // Ghi đè hàm của NguoiDung
        public override string ThongTinLienHe()
        {
            return $"Nhà Tài Trợ: {hoTen} - Lĩnh vực: {LinhVucHoatDong} - Đại diện: {NguoiDaiDien} ({sdt})";
        }

        // Ghi đè hàm của BaseEntity
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                Console.WriteLine("Tên nhà tài trợ không được để trống!");
                return false;
            }
            if (email == null || !email.Contains("@")) return false;
            return true;
        }
    }
}
