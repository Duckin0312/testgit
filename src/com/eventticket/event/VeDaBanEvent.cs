using System;
using EventTicket.Model.Entity;

namespace EventTicket.Event
{
    public class VeDaBanEvent
    {
        public Ve Ve { get; }
        public KhachHang KhachHang { get; }
        public DateTime ThoiGianBan { get; }

        public VeDaBanEvent(Ve ve, KhachHang khachHang, DateTime? thoiGianBan)
        {
            Ve = ve;
            KhachHang = khachHang;
            ThoiGianBan = thoiGianBan ?? DateTime.Now;
        }

        public override string ToString()
        {
            string maVe = (Ve != null) ? Ve.MaVe : "N/A";
            string maKh = (KhachHang != null)
                ? KhachHang.MaKhachHang
                : (Ve != null ? Ve.MaKhachHang : "N/A");
            return "Ve da ban Event: " + maVe + ", Khach hang: " + maKh + ", Thoi gian: " + ThoiGianBan;
        }
    }
}
