using System.Collections.Generic;
using System.Linq;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;

namespace JffCsharpTools.Domain.Model
{
    public class PaginationModel<TEntity, TFilter> where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TFilter>, new()
    {
        public int Page { get; set; }
        public int CountPage { get; set; }
        public string Order { get; set; }
        public string TypeOrder { get; set; }
        public int Total { get; set; }
        public TFilter Filter { get; set; }
        public IEnumerable<TEntity> List { get; set; } = Enumerable.Empty<TEntity>();
    }
}