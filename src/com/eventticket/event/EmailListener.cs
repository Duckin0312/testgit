using System;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;

namespace EventTicket.Event
{
    public class EmailListener : IEventListener<VeDaBanEvent>
    {
        public void OnEvent(VeDaBanEvent evt)
        {
            if (evt == null) return;
            Ve ve = evt.Ve;
            KhachHang kh = evt.KhachHang;

            string email = (kh != null && kh.Email != null) ? kh.Email : "N/A";
            string tenKhach = (kh != null) ? kh.HoTen : "Khach hang";
            string maVe = (ve != null) ? ve.MaVe : "N/A";

            Console.WriteLine("[EmailService] Dang gui email xac nhan den: " + email);
            Console.WriteLine("Tieu de: [EventTicket] Xac nhan dat ve thanh cong #" + maVe);
            Console.WriteLine(" Noi dung: Xin chao " + tenKhach + ", cam on ban da dat ve tai he thong.");
        }
    }
}
