using System;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class LichBieuDien : BaseEntity
    {
        public string MaLich { get; set; }
        public string MaSuKien { get; set; }
        public string MaNgheSi { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int ThuTuBieuDien { get; set; }
        public string NoiDungBieuDien { get; set; }

        public LichBieuDien() : base()
        {
        }

        public LichBieuDien(string maLich, string maSuKien, string maNgheSi,
                            DateTime? thoiGianBatDau, DateTime? thoiGianKetThuc,
                            int thuTuBieuDien, string noiDungBieuDien)
        {
            MaLich = maLich;
            MaSuKien = maSuKien;
            MaNgheSi = maNgheSi;
            ThoiGianBatDau = thoiGianBatDau;
            ThoiGianKetThuc = thoiGianKetThuc;
            ThuTuBieuDien = thuTuBieuDien;
            NoiDungBieuDien = noiDungBieuDien;
        }

        // thời lượng biểu diễn (theo phút)
        public long TinhThoiLuong()
        {
            if (ThoiGianBatDau == null || ThoiGianKetThuc == null)
            {
                return 0;
            }
            return (long)(ThoiGianKetThuc.Value - ThoiGianBatDau.Value).TotalMinutes;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(MaLich))
            {
                Console.WriteLine("Lỗi: Mã lịch biểu diễn không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaSuKien))
            {
                Console.WriteLine("Lỗi: Mã sự kiện không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaNgheSi))
            {
                Console.WriteLine("Lỗi: Mã nghệ sĩ không được để trống!");
                return false;
            }
            else if (ThoiGianBatDau == null)
            {
                Console.WriteLine("Lỗi: Thời gian bắt đầu biểu diễn không hợp lệ!");
                return false;
            }
            else if (ThoiGianKetThuc == null)
            {
                Console.WriteLine("Lỗi: Thời gian kết thúc biểu diễn không hợp lệ!");
                return false;
            }
            else if (ThoiGianKetThuc <= ThoiGianBatDau)
            {
                Console.WriteLine("Lỗi: Thời gian kết thúc phải sau thời gian bắt đầu!");
                return false;
            }
            else if (ThuTuBieuDien <= 0)
            {
                Console.WriteLine("Lỗi: Thứ tự biểu diễn phải lớn hơn 0!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(NoiDungBieuDien))
            {
                Console.WriteLine("Lỗi: Nội dung biểu diễn không được để trống!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return MaLich + " - " + MaNgheSi + " - " + MaSuKien +
                   " sẽ biểu diễn vào lúc " + ThoiGianBatDau + " cho đến " + ThoiGianKetThuc +
                   " theo thứ tự thứ " + ThuTuBieuDien + " với nội dung " + NoiDungBieuDien;
        }
    }
}
