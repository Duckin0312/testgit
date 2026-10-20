using System;

namespace EventTicket.Model.Enums
{
    public enum LoaiVe
    {
        VIP,
        PREMIUM,
        STANDARD
    }

    public static class LoaiVeExtensions
    {
        public static string MoTa(this LoaiVe loai) => loai switch
        {
            LoaiVe.VIP => "Vé VIP",
            LoaiVe.PREMIUM => "Vé cao cấp",
            LoaiVe.STANDARD => "Vé thường",
            _ => throw new ArgumentOutOfRangeException(nameof(loai), loai, null)
        };

        public static double HeSoGia(this LoaiVe loai) => loai switch
        {
            LoaiVe.VIP => 3.0,
            LoaiVe.PREMIUM => 2.0,
            LoaiVe.STANDARD => 1.0,
            _ => throw new ArgumentOutOfRangeException(nameof(loai), loai, null)
        };
    }
}
