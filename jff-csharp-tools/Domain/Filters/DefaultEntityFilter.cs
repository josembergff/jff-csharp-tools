using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JffCsharpTools.Domain.Entity;

namespace JffCsharpTools.Domain.Filters
{
    public class DefaultEntityFilter<TEntity> : DefaultFilter<DefaultEntity<TEntity>> where TEntity : DefaultEntity<TEntity>, new()
    {

        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public DateTime? UpdatedAtStart { get; set; }
        public DateTime? UpdatedAtEnd { get; set; }

        public override Expression<Func<DefaultEntity<TEntity>, bool>> Where()
        {
            if (_where != null)
            {
                return _where;
            }

            var whereList = new List<Expression<Func<DefaultEntity<TEntity>, bool>>>();

            if (CreatedAtStart != null && CreatedAtStart > DateTime.MinValue && CreatedAtStart < DateTime.MaxValue)
                whereList.Add(x => x.CreatedAt.Date >= CreatedAtStart.Value.Date);

            if (CreatedAtEnd != null && CreatedAtEnd > DateTime.MinValue && CreatedAtEnd < DateTime.MaxValue)
                whereList.Add(x => x.CreatedAt.Date <= CreatedAtEnd.Value.Date);

            if (UpdatedAtStart != null && UpdatedAtStart > DateTime.MinValue && UpdatedAtStart < DateTime.MaxValue)
                whereList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date >= (UpdatedAtStart ?? DateTime.Now).Date);

            if (UpdatedAtEnd != null && UpdatedAtEnd > DateTime.MinValue && UpdatedAtEnd < DateTime.MaxValue)
                whereList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date <= (UpdatedAtEnd ?? DateTime.Now).Date);

            Expression<Func<DefaultEntity<TEntity>, bool>> where = PredicateBuilderFilter.True<DefaultEntity<TEntity>>();
            foreach (Expression<Func<DefaultEntity<TEntity>, bool>> predicate in whereList)
            {
                where = PredicateBuilderFilter.And(where, predicate);
            }

            return _where = where;
        }
    }
}