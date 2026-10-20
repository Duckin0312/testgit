using System;
using System.Collections.Generic;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class NgheSi : NguoiDungBenNgoai
    {
        public string TenNgheSi { get; set; }
        public string TheLoaiBieuDien { get; set; }
        public List<LichBieuDien> LichBieuDien { get; set; } = new List<LichBieuDien>();

        public NgheSi() : base()
        {
        }

        public NgheSi(string maNgheSi, string tenNgheSi, string theLoaiBieuDien) : base()
        {
            Id = maNgheSi;
            TenNgheSi = tenNgheSi;
            TheLoaiBieuDien = theLoaiBieuDien;
        }

        public string MaNgheSi
        {
            get => Id;
            set => Id = value;
        }

        // Thêm lịch biểu diễn
        public void ThemLich(LichBieuDien lich)
        {
            if (lich != null)
            {
                LichBieuDien.Add(lich);
            }
        }

        // Lấy danh sách lịch biểu diễn sắp tới
        public List<LichBieuDien> GetSuKienSapToi()
        {
            List<LichBieuDien> ketQua = new List<LichBieuDien>();
            DateTime hienTai = DateTime.Now;
            foreach (LichBieuDien lich in LichBieuDien)
            {
                if (lich != null && lich.ThoiGianBatDau != null && lich.ThoiGianBatDau > hienTai)
                {
                    ketQua.Add(lich);
                }
            }
            return ketQua;
        }

        public override IEnumerable<object> GetLichSuGiaoDich()
        {
            return new List<object>();
        }

        public override string ThongTinLienHe()
        {
            return "Nghệ sĩ: " + TenNgheSi + " - Mã: " + MaNgheSi
                   + " - Thể loại: " + TheLoaiBieuDien + " - Email: " + email + " - SĐT: " + sdt;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Console.WriteLine("Lỗi: Mã nghệ sĩ không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TenNgheSi))
            {
                Console.WriteLine("Lỗi: Tên nghệ sĩ không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TheLoaiBieuDien))
            {
                Console.WriteLine("Lỗi: Thể loại biểu diễn không được để trống!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return MaNgheSi + " - " + TenNgheSi + " - " + TheLoaiBieuDien;
        }
    }
}
