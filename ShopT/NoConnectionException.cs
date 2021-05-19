using System;

namespace ShopT
{
    public class NoConnectionException : Exception
    {
        public NoConnectionException() : base() { }
        public NoConnectionException(string message) : base(message) { }
        public NoConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
