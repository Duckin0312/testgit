using System;
using System.Globalization;
using System.Text;

namespace EventTicket
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // In đúng tiếng Việt có dấu trên console
            Console.OutputEncoding = Encoding.UTF8;

            // Dùng InvariantCulture để số in dạng "1,000" giống %,d của Java
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            TestQueryRunner.ChayTatCaTruyVan();

            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }
    }
}
