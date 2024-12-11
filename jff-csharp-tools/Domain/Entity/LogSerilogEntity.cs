using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace JffCsharpTools.Domain.Entity
{
    [Table("AspNetApiWebAuthLogs")]
    public class LogSerilogEntity
    {
        [Column("id", TypeName = "INT")]
        public int Id { get; set; }

        [Column("Timestamp", TypeName = "VARCHAR(100)")]
        public string Timestamp { get; set; }

        [Column("Level", TypeName = "VARCHAR(15)")]
        public string Level { get; set; }

        [Column("Template", TypeName = "TEXT")]
        public string Template { get; set; }

        [Column("Message", TypeName = "TEXT")]
        public string Message { get; set; }

        [Column("Exception", TypeName = "TEXT")]
        public string Exception { get; set; }

        [Column("Properties", TypeName = "TEXT")]
        public string Properties { get; set; }

        [Column("_ts", TypeName = "TIMESTAMP")]
        [DefaultValue("CURRENT_TIMESTAMP")]
        public DateTime Ts { get; set; }

        public override string ToString()
        {
            return $"[{Timestamp}] {Level}: {Message} {Exception}";
        }
    }
}