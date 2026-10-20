using System;

namespace EventTicket.Exception
{
    /// <summary>
    /// Ngoai le dai dien cho cac loi lien quan den tang du lieu (Database / Repository)
    /// </summary>
    public class DatabaseException : System.Exception
    {
        public string MaLoi { get; set; }

        public DatabaseException() : base() { }

        public DatabaseException(string message) : base(message) { }

        public DatabaseException(string message, System.Exception cause) : base(message, cause) { }

        public DatabaseException(System.Exception cause) : base(cause?.Message, cause) { }

        public DatabaseException(string maLoi, string message) : base(message)
        {
            MaLoi = maLoi;
        }
    }
}
