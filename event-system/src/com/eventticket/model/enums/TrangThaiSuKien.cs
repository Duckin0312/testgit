using System;

namespace EventTicket.Model.Enums
{
    public enum TrangThaiSuKien
    {
        UPCOMING,
        ONGOING,
        COMPLETED,
        CANCELLED
    }

    public static class TrangThaiSuKienExtensions
    {
        public static string MoTa(this TrangThaiSuKien tt) => tt switch
        {
            TrangThaiSuKien.UPCOMING => "SẮP DIỄN RA",
            TrangThaiSuKien.ONGOING => "ĐANG DIỄN RA",
            TrangThaiSuKien.COMPLETED => "ĐÃ KẾT THÚC",
            TrangThaiSuKien.CANCELLED => "ĐÃ HỦY",
            _ => throw new ArgumentOutOfRangeException(nameof(tt), tt, null)
        };
    }
}
