
using System;
using System.Collections.Generic;
using JffCsharpTools.Common;

namespace Jff.Csharp.Tools.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public DomainException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        public DomainException(List<string> errors)
            : base("Domain validation failed")
        {
            Errors = errors;
        }

        public DomainException(Result result, Exception innerException)
            : base(result?.Error ?? "An error domain occurred", innerException)
        {
            Errors = result?.Errors ?? new List<string> { result?.Error ?? "Domain validation failed" };
        }
    }
}