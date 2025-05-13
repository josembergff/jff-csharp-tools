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