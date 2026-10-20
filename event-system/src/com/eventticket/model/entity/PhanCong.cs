using System;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class PhanCong : BaseEntity
    {
        public string MaSuKien { get; set; }
        public string MaNV { get; set; }
        public string NhiemVu { get; set; }

        public PhanCong() : base()
        {
        }

        public PhanCong(string maSuKien, string maNV, string nhiemVu)
        {
            MaSuKien = maSuKien;
            MaNV = maNV;
            NhiemVu = nhiemVu;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(MaSuKien))
            {
                Console.WriteLine("Lỗi: Mã sự kiện không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaNV))
            {
                Console.WriteLine("Lỗi: Mã nhân viên không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(NhiemVu))
            {
                Console.WriteLine("Lỗi: Nhiệm Vụ không hợp lệ");
                return false;
            }
            return true;
        }
    }
}
