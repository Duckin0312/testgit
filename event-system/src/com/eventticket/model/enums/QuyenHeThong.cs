using System;

namespace EventTicket.Model.Enums
{
    public enum QuyenHeThong
    {
        ADMIN,
        QUAN_LY,
        NHAN_VIEN
    }

    public static class QuyenHeThongExtensions
    {
        public static string ChucVu(this QuyenHeThong quyen) => quyen switch
        {
            QuyenHeThong.ADMIN => "ADMIN",
            QuyenHeThong.QUAN_LY => "Quản lý",
            QuyenHeThong.NHAN_VIEN => "Nhân viên",
            _ => throw new ArgumentOutOfRangeException(nameof(quyen), quyen, null)
        };
    }
}
