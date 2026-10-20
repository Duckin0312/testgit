using System;
using System.Globalization;
using System.Linq;

namespace EventTicket.Util
{
    public static class FormatUtil
    {
        private static readonly NumberFormatInfo TienFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = ".",
            NumberDecimalSeparator = ","
        };

        // Dinh dang so tien sang chuoi VND (VD: 250000 -> "250.000 VND")
        public static string DinhDangTien(decimal? soTien)
        {
            if (soTien == null) return "0 VND";
            // Java DecimalFormat lam tron HALF_EVEN, nen lam tron tuong tu de ket qua giong nhau
            decimal lamTron = Math.Round(soTien.Value, 0, MidpointRounding.ToEven);
            return lamTron.ToString("#,##0", TienFormat) + " VND";
        }

        public static string DinhDangTien(double soTien)
        {
            return DinhDangTien((decimal?)(decimal)soTien);
        }

        // Chuan hoa chuoi ho ten (viet hoa chu cai dau moi tu, xoa khoang trang thua)
        // VD: "  nguyen   van   a " -> "Nguyen Van A"
        public static string ChuanHoaHoTen(string? hoTen)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                return "";
            }

            // Split(null) tach theo moi ky tu khoang trang
            var tu = hoTen.Trim().ToLowerInvariant()
                          .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", tu.Select(t => char.ToUpperInvariant(t[0]) + t.Substring(1)));
        }
    }
}
