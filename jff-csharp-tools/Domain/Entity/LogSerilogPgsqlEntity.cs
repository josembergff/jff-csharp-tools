using System;

namespace JFF.Domain.Entity
{
    public class LogSerilogPgsqlEntity
    {
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
        public string LogEvent { get; set; }
    }
}
