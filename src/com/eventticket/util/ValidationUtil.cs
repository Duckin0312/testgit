using System.Text.RegularExpressions;

namespace EventTicket.Util
{
    public static class ValidationUtil
    {
        // Ten tieng Viet co dau, tu 2 den 50 ky tu
        private static readonly Regex TenRegex = new Regex(@"^[\p{L} ]{2,50}\z", RegexOptions.Compiled);

        // So dien thoai Viet Nam: 10 chu so, bat dau bang 0
        private static readonly Regex SdtRegex = new Regex(@"^0[0-9]{9}\z", RegexOptions.Compiled);

        // Email hop le
        private static readonly Regex EmailRegex = new Regex(@"^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\z", RegexOptions.Compiled);

        public static bool LaTenHopLe(string? hoTen)
        {
            return hoTen != null && TenRegex.IsMatch(hoTen.Trim());
        }

        public static bool LaSdtHopLe(string? sdt)
        {
            return sdt != null && SdtRegex.IsMatch(sdt.Trim());
        }

        public static bool LaEmailHopLe(string? email)
        {
            return email != null && EmailRegex.IsMatch(email.Trim());
        }

        public static bool LaMatKhauHopLe(string? matKhau)
        {
            return matKhau != null && matKhau.Length >= 6;
        }
    }
}
