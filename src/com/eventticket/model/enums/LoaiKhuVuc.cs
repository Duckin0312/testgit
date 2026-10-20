using System;

namespace EventTicket.Model.Enums
{
    public enum LoaiKhuVuc
    {
        SAN_KHAU,
        KHAN_GIA_VIP,
        KHAN_GIA_THUONG,
        VIP_LOUNGE
    }

    public static class LoaiKhuVucExtensions
    {
        public static string MoTa(this LoaiKhuVuc loai) => loai switch
        {
            LoaiKhuVuc.SAN_KHAU => "Khu vực sân khấu",
            LoaiKhuVuc.KHAN_GIA_VIP => "Khu vực cho khách VIP",
            LoaiKhuVuc.KHAN_GIA_THUONG => "Khu vực cho khách thường",
            LoaiKhuVuc.VIP_LOUNGE => "Phòng chờ VIP",
            _ => throw new ArgumentOutOfRangeException(nameof(loai), loai, null)
        };
    }
}
