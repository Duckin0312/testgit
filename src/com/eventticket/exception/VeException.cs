using System;

namespace EventTicket.Exception
{
    /// <summary>
    /// Ngoai le nghiep vu danh rieng cho cac thao tac ve (ban ve, huy ve, kiem tra ve)
    /// </summary>
    public class VeException : System.Exception
    {
        public string MaLoi { get; set; }

        public VeException() : base() { }

        public VeException(string message) : base(message) { }

        public VeException(string message, System.Exception cause) : base(message, cause) { }

        public VeException(System.Exception cause) : base(cause?.Message, cause) { }

        public VeException(string maLoi, string message) : base(message)
        {
            MaLoi = maLoi;
        }
    }
}
