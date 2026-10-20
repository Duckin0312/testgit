using System;
using System.Globalization;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class TaiTro : BaseEntity
    {
        public NhaTaiTro NhaTaiTro { get; set; }
        public SuKien SuKien { get; set; }

        public decimal? SoTienTaiTro { get; set; }
        public string GoiTaiTro { get; set; } // VD: "Kim Cương", "Vàng", "Bạc", "Đồng"
        public DateTime NgayKyKet { get; set; }

        // constructor rỗng
        public TaiTro() : base()
        {
            NgayKyKet = DateTime.Now;
        }

        // constructor đầy đủ
        public TaiTro(NhaTaiTro nhaTaiTro, SuKien suKien, decimal? soTien, string goi) : base()
        {
            NhaTaiTro = nhaTaiTro;
            SuKien = suKien;
            SoTienTaiTro = soTien;
            GoiTaiTro = goi;
            NgayKyKet = DateTime.Now;
        }

        public override bool Validate()
        {
            if (NhaTaiTro == null)
            {
                Console.WriteLine("Lỗi: Giao dịch phải có Nhà Tài Trợ!");
                return false;
            }
            if (SuKien == null)
            {
                Console.WriteLine("Lỗi: Phải chỉ định rõ tài trợ cho Sự Kiện nào!");
                return false;
            }
            if (SoTienTaiTro == null || SoTienTaiTro.Value <= 0m)
            {
                Console.WriteLine("Lỗi: Số tiền tài trợ phải lớn hơn 0!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"Giao dịch Tài trợ: {NhaTaiTro.HoTen} tài trợ {SoTienTaiTro?.ToString(CultureInfo.InvariantCulture)} VND (Gói {GoiTaiTro}) cho sự kiện {SuKien.TenSuKien}";
        }
    }
}
