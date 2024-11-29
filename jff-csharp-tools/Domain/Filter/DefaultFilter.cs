using System;

namespace JffCsharpTools.Domain.Filter
{
    public class DefaultFilter
    {
        public int Id { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}