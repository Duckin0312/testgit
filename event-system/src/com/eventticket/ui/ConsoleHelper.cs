using System;

namespace EventTicket.UI
{
    /// <summary>Thay the cho java.util.Scanner.nextLine() + trim().</summary>
    public static class ConsoleHelper
    {
        public static string DocDong()
        {
            return (Console.ReadLine() ?? string.Empty).Trim();
        }
    }
}
