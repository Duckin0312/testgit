using System;
using EventTicket.Model.Base;
using EventTicket.Model.Enums;

namespace EventTicket.Model.Entity
{
    public class SuKien : BaseEntity
    {
        public string TenSuKien { get; set; }
        public TrangThaiSuKien? TrangThaiSuKien { get; set; }
        public DateTime? ThoiGianDienRa { get; set; }
        public DiaDiem DiaDiem { get; set; }

        public SuKien() : base()
        {
        }

        public SuKien(string maSuKien, string tenSuKien, TrangThaiSuKien? trangThaiSuKien,
                      DateTime? thoiGianDienRa, DiaDiem diaDiem)
        {
            Id = maSuKien;
            TenSuKien = tenSuKien;
            TrangThaiSuKien = trangThaiSuKien;
            ThoiGianDienRa = thoiGianDienRa;
            DiaDiem = diaDiem;
        }

        // chỉ có getter
        public string MaSuKien => Id;

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Console.WriteLine("Lỗi: ID sự kiện không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TenSuKien))
            {
                Console.WriteLine("Lỗi: Tên sự kiện không được để trống!");
                return false;
            }
            else if (TrangThaiSuKien == null)
            {
                Console.WriteLine("Lỗi: Trạng thái sự kiện không hợp lệ!");
                return false;
            }
            else if (ThoiGianDienRa == null)
            {
                Console.WriteLine("Lỗi: Thời gian diễn ra sự kiện không hợp lệ!");
                return false;
            }
            else if (DiaDiem == null)
            {
                Console.WriteLine("Lỗi: Địa điểm sự kiện không hợp lệ!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return Id + " - " + TenSuKien;
        }
    }
}
