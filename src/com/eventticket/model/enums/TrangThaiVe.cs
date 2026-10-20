using System;

namespace EventTicket.Model.Enums
{
    public enum TrangThaiVe
    {
        RESERVED,
        SOLD,
        CANCELLED,
        EXPIRED
    }

    public static class TrangThaiVeExtensions
    {
        public static string MoTa(this TrangThaiVe tt) => tt switch
        {
            TrangThaiVe.RESERVED => "Đã đặt chỗ",
            TrangThaiVe.SOLD => "Đã bán",
            TrangThaiVe.CANCELLED => "Đã hủy",
            TrangThaiVe.EXPIRED => "Hết hạn",
            _ => throw new ArgumentOutOfRangeException(nameof(tt), tt, null)
        };
    }
}
