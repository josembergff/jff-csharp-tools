using System;

namespace JffCsharpTools.Domain.Entity
{
    /// <summary>
    /// Entity representing a log entry for Serilog structured logging.
    /// Maps to the AspNetApiWebAuthLogs database table and contains
    /// all necessary fields for storing structured log data from Serilog.
    /// </summary>
    public class LogSerilogEntity
    {
        /// <summary>
        /// Unique identifier for the log entry
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The message template used in the log entry
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The rendered message after applying the template
        /// </summary>
        public string MessageTemplate { get; set; }
        /// <summary>
        /// The log level (e.g., Information, Error)
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// The timestamp of when the log entry was created
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The exception message or stack trace if an exception occurred
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// The structured properties associated with the log entry
        /// JSONB => pode ser string ou Dictionary<string, object>
        /// </summary>
        public string Properties { get; set; }
        /// <summary>
        /// The log event identifier
        /// </summary>
        public string LogEvent { get; set; }

        /// <summary>
        /// Returns a formatted string representation of the log entry
        /// including timestamp, level, message and exception information
        /// </summary>
        /// <returns>Formatted log entry string</returns>
        public override string ToString()
        {
            return $"[{Timestamp}] {Level}: {Message} {Exception}";
        }
    }
}