using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace JffCsharpTools.Domain.Entity
{
    /// <summary>
    /// Entity representing a log entry for Serilog structured logging.
    /// Maps to the AspNetApiWebAuthLogs database table and contains
    /// all necessary fields for storing structured log data from Serilog.
    /// </summary>
    [Table("AspNetApiWebAuthLogs")]
    public class LogSerilogEntity
    {
        /// <summary>
        /// Unique identifier for the log entry
        /// </summary>
        [Column("id", TypeName = "INT")]
        public int Id { get; set; }

        /// <summary>
        /// String representation of when the log event occurred
        /// </summary>
        [Column("Timestamp", TypeName = "VARCHAR(100)")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Log level (Debug, Information, Warning, Error, Fatal, etc.)
        /// </summary>
        [Column("Level", TypeName = "VARCHAR(15)")]
        public string Level { get; set; }

        /// <summary>
        /// Message template used for structured logging
        /// </summary>
        [Column("Template", TypeName = "TEXT")]
        public string Template { get; set; }

        /// <summary>
        /// Rendered log message with parameters replaced
        /// </summary>
        [Column("Message", TypeName = "TEXT")]
        public string Message { get; set; }

        /// <summary>
        /// Exception details if the log entry represents an exception
        /// </summary>
        [Column("Exception", TypeName = "TEXT")]
        public string Exception { get; set; }

        /// <summary>
        /// JSON representation of structured properties associated with the log event
        /// </summary>
        [Column("Properties", TypeName = "TEXT")]
        public string Properties { get; set; }

        /// <summary>
        /// Database timestamp indicating when the log entry was persisted
        /// Automatically set to current timestamp on insert
        /// </summary>
        [Column("_ts", TypeName = "TIMESTAMP")]
        [DefaultValue("CURRENT_TIMESTAMP")]
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