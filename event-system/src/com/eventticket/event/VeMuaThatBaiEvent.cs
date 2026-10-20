using System;

namespace EventTicket.Event
{
    public class VeMuaThatBaiEvent
    {
        public string LyDo { get; }
        public string MaKhachHang { get; }
        public string MaSuKien { get; }
        public DateTime ThoiGian { get; }

        public VeMuaThatBaiEvent(string lyDo, string maKhachHang, string maSuKien)
        {
            LyDo = lyDo;
            MaKhachHang = maKhachHang;
            MaSuKien = maSuKien;
            ThoiGian = DateTime.Now;
        }

        public VeMuaThatBaiEvent(string lyDo) : this(lyDo, null, null) { }

        public override string ToString()
        {
            return "Mua vé thất bại: " + LyDo + ", Khách hàng: " + MaKhachHang +
                   ", mã sự kiện: " + MaSuKien + ", thời gian: " + ThoiGian;
        }
    }
}
