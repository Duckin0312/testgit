using System;

namespace EventTicket.Model.Enums
{
    public enum LoaiKhachHang
    {
        THUONG,
        THAN_THIET,
        VIP
    }

    public static class LoaiKhachHangExtensions
    {
        public static string MoTa(this LoaiKhachHang loai) => loai switch
        {
            LoaiKhachHang.THUONG => "Khách thường",
            LoaiKhachHang.THAN_THIET => "Khách thân thiết",
            LoaiKhachHang.VIP => "Khách VIP",
            _ => throw new ArgumentOutOfRangeException(nameof(loai), loai, null)
        };
    }
}
