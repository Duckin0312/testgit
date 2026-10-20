using System;
using System.Threading;

namespace EventTicket.Util
{
    public static class IdGenerator
    {
        private static int _demKhachHang = 100;
        private static int _demVe = 100;
        private static int _demSuKien = 10;
        private static int _demNhanVien = 10;

        public static string SinhMaKhachHang()
        {
            return "KH" + Interlocked.Increment(ref _demKhachHang).ToString("D4");
        }

        public static string SinhMaVe()
        {
            return "VE" + Interlocked.Increment(ref _demVe).ToString("D5");
        }

        public static string SinhMaSuKien()
        {
            return "SK" + Interlocked.Increment(ref _demSuKien).ToString("D3");
        }

        public static string SinhMaNhanVien()
        {
            return "NV" + Interlocked.Increment(ref _demNhanVien).ToString("D3");
        }

        // Sinh ma ngau nhien duy nhat (GUID rut gon) cho ma giao dich hoac ma xac thuc
        public static string SinhMaNgauNhien(string? prefix)
        {
            string randomStr = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant();
            return (prefix ?? "") + randomStr;
        }
    }
}
