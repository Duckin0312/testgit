using System;
using System.Globalization;

namespace EventTicket.Util
{
    public static class DateUtil
    {
        public const string DatePattern = "dd/MM/yyyy";
        public const string DateTimePattern = "dd/MM/yyyy HH:mm";

        public static string FormatNgay(DateOnly? date)
        {
            if (date == null) return "N/A";
            return date.Value.ToString(DatePattern, CultureInfo.InvariantCulture);
        }

        public static string FormatNgayGio(DateTime? dateTime)
        {
            if (dateTime == null) return "N/A";
            return dateTime.Value.ToString(DateTimePattern, CultureInfo.InvariantCulture);
        }

        public static DateOnly? ParseNgay(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;
            return DateOnly.ParseExact(dateStr.Trim(), DatePattern, CultureInfo.InvariantCulture);
        }

        public static DateTime? ParseNgayGio(string? dateTimeStr)
        {
            if (string.IsNullOrWhiteSpace(dateTimeStr)) return null;
            return DateTime.ParseExact(dateTimeStr.Trim(), DateTimePattern, CultureInfo.InvariantCulture);
        }
    }
}
