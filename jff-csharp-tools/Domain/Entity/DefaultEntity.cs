using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using JffCsharpTools.Domain.Filters;

namespace JffCsharpTools.Domain.Entity
{
    public class DefaultEntity<TEntity> where TEntity : DefaultEntity<TEntity>, new()
    {
        [Key]
        public int Id { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public DateTime? UpdatedAtStart { get; set; }
        public DateTime? UpdatedAtEnd { get; set; }

        protected Expression<Func<TEntity, bool>> GetDefaultFilter()
        {
            var filterList = new List<Expression<Func<TEntity, bool>>>();
            Expression<Func<TEntity, bool>> filter = PredicateBuilderFilter.True<TEntity>();

            if (Id > 0)
                filterList.Add(x => x.Id == Id);

            if (CreatorUserId > 0)
                filterList.Add(x => x.CreatorUserId == CreatorUserId);

            if (CreatedAt > DateTime.MinValue && CreatedAt < DateTime.MaxValue)
                filterList.Add(x => x.CreatedAt.Date == CreatedAt.Date);

            if (UpdatedAt != null && UpdatedAt > DateTime.MinValue && UpdatedAt < DateTime.MaxValue)
                filterList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date == (UpdatedAt ?? DateTime.Now).Date);

            if (CreatedAtStart != null && CreatedAtStart > DateTime.MinValue && CreatedAtStart < DateTime.MaxValue)
                filterList.Add(x => x.CreatedAt.Date >= CreatedAtStart.Value.Date);

            if (CreatedAtEnd != null && CreatedAtEnd > DateTime.MinValue && CreatedAtEnd < DateTime.MaxValue)
                filterList.Add(x => x.CreatedAt.Date <= CreatedAtEnd.Value.Date);

            if (UpdatedAtStart != null && UpdatedAtStart > DateTime.MinValue && UpdatedAtStart < DateTime.MaxValue)
                filterList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date >= (UpdatedAtStart ?? DateTime.Now).Date);

            if (UpdatedAtEnd != null && UpdatedAtEnd > DateTime.MinValue && UpdatedAtEnd < DateTime.MaxValue)
                filterList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date <= (UpdatedAtEnd ?? DateTime.Now).Date);

            foreach (Expression<Func<TEntity, bool>> predicado in filterList)
            {
                filter = predicado.And(filter);
            }

            return filter;
        }

        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            return GetDefaultFilter();
        }
    }
}