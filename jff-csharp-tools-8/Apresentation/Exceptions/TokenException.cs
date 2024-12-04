using System;

namespace JffCsharpTools8.Apresentation.Exceptions
{
    public class TokenException : Exception
    {
        public TokenException() : base("Token not found.")
        {
        }

        public TokenException(string message) : base(message)
        {
        }

        public TokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}