using System;
using EventTicket.Model.Entity;

namespace EventTicket.Event
{
    public class SuKienHuyEvent
    {
        public SuKien SuKien { get; }
        public string LyDoHuy { get; }
        public DateTime ThoiGianHuy { get; }

        public SuKienHuyEvent(SuKien suKien, string lyDoHuy)
        {
            SuKien = suKien;
            LyDoHuy = lyDoHuy;
            ThoiGianHuy = DateTime.Now;
        }

        public override string ToString()
        {
            string tenSk = (SuKien != null) ? SuKien.TenSuKien : "N/A";
            return $"Su kien [{tenSk}] da bi huy luc {ThoiGianHuy}. Ly do: {LyDoHuy}";
        }
    }
}
