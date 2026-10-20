using System;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;

namespace EventTicket.Event
{
    public class ThongBaoListener : IEventListener<VeDaBanEvent>
    {
        public void OnEvent(VeDaBanEvent evt)
        {
            if (evt == null) return;
            Ve ve = evt.Ve;
            KhachHang kh = evt.KhachHang;

            string sdt = (kh != null && kh.Sdt != null) ? kh.Sdt : "N/A";
            string maVe = (ve != null) ? ve.MaVe : "N/A";

            Console.WriteLine("[Notification] Da gui tin nhan SMS den SĐT: " + sdt);
            Console.WriteLine("NoiDung: Dat ve thanh cong! Ma ve cua ban la: " + maVe);
        }
    }
}
