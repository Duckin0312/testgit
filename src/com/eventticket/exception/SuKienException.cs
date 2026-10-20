using System;

namespace EventTicket.Exception
{
    /// <summary>
    /// Ngoai le nghiep vu danh rieng cho cac thao tac tren SuKien
    /// </summary>
    public class SuKienException : System.Exception
    {
        public string MaLoi { get; set; }

        public SuKienException() : base() { }

        public SuKienException(string message) : base(message) { }

        public SuKienException(string message, System.Exception cause) : base(message, cause) { }

        public SuKienException(System.Exception cause) : base(cause?.Message, cause) { }

        public SuKienException(string maLoi, string message) : base(message)
        {
            MaLoi = maLoi;
        }
    }
}
