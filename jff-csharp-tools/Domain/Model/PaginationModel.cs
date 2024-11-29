using System.Collections.Generic;
using System.Linq;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filter;

namespace JffCsharpTools.Domain.Model
{
    public class PaginationModel<TEntity> where TEntity : DefaultEntity<TEntity>, new()
    {
        public int Page { get; set; }
        public int CountPage { get; set; }
        public string Order { get; set; }
        public string TypeOrder { get; set; }
        public int Total { get; set; }
        public DefaultFilter Filter { get; set; }
        public IEnumerable<TEntity> List { get; set; } = Enumerable.Empty<TEntity>();
    }
}