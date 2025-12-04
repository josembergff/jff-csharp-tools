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
        public long Id { get; set; }

        /// <summary>
        /// String representation of when the log event occurred
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Log level (Debug, Information, Warning, Error, Fatal, etc.)
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Message template used for structured logging
        /// </summary>
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Rendered log message with parameters replaced
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Exception details if the log entry represents an exception
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// JSON representation of structured properties associated with the log event
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// Timestamp indicating when the log entry was created
        /// </summary>
        public DateTime Ts { get; set; }

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