using System;
using EventTicket.Interfaces;

namespace EventTicket.Event
{
    public class LogListener : IEventListener<object>
    {
        public void OnEvent(object evt)
        {
            if (evt == null) return;

            if (evt is SuKienHuyEvent skHuy)
            {
                Console.WriteLine("[System Audit Log] [" + DateTime.Now + "] CANH BAO: Su kien da bi huy!");
                Console.WriteLine("                   Chi tiet: " + skHuy);
            }
            else if (evt is VeMuaThatBaiEvent thatBai)
            {
                Console.WriteLine("[System Alert Log] [" + DateTime.Now + "] CANH BAO: Giao dich mua ve that bai!");
                Console.WriteLine("                   Chi tiet: " + thatBai);
            }
        }
    }
}
