#nullable enable
using System;

namespace JffCsharpTools.Domain.Model
{
    public class AppLog
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Exception { get; set; }
        public string? Properties { get; set; }
    }
}