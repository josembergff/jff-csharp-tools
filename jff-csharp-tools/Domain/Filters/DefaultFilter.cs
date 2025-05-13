using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JffCsharpTools.Domain.Entity;

namespace JffCsharpTools.Domain.Filters
{
    public class DefaultFilter<TEntity> where TEntity : DefaultEntity<TEntity>, new()
    {
        public int Id { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public DateTime? UpdatedAtStart { get; set; }
        public DateTime? UpdatedAtEnd { get; set; }

        private int _pageSize;
        private int _pageNumber;
        protected string _orderBy;
        protected bool _asc;
        public const int MAX_RESULT_SIZE = 10;
        protected Expression<Func<TEntity, bool>> _where;
        public bool IgnorePagination = false;

        public DefaultFilter()
        {
            _pageSize = MAX_RESULT_SIZE;
            _pageNumber = 1;
        }

        public int Count
        {
            get { return _pageSize; }
            set
            {
                if (value >= 1)
                    _pageSize = value;
                else
                    _pageSize = 10;
            }
        }

        public int Page
        {
            get { return _pageNumber; }
            set
            {
                if (value >= 1)
                    _pageNumber = value;
                else
                    _pageNumber = 1;
            }
        }

        public int SkipTotal
        {
            get { return IgnorePagination ? 0 : (Page - 1) * Count; }
        }

        public string OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        public bool Asc
        {
            get { return _asc; }
            set { _asc = value; }
        }

        public virtual Expression<Func<TEntity, bool>> Where()
        {
            if (_where == null)
            {
                List<Expression<Func<TEntity, bool>>> whereList = new List<Expression<Func<TEntity, bool>>>();
                Expression<Func<TEntity, bool>> where = PredicateBuilderFilter.True<TEntity>();

                if (Id > 0)
                    whereList.Add(x => x.Id == Id);

                if (CreatorUserId > 0)
                    whereList.Add(x => x.CreatorUserId == CreatorUserId);

                if (CreatedAtStart != null && CreatedAtStart > DateTime.MinValue && CreatedAtStart < DateTime.MaxValue)
                    whereList.Add(x => x.CreatedAt.Date >= CreatedAtStart.Value.Date);

                if (CreatedAtEnd != null && CreatedAtEnd > DateTime.MinValue && CreatedAtEnd < DateTime.MaxValue)
                    whereList.Add(x => x.CreatedAt.Date <= CreatedAtEnd.Value.Date);

                foreach (Expression<Func<TEntity, bool>> predicate in whereList)
                {
                    where = PredicateBuilderFilter.And(where, predicate);
                }

                _where = where;
            }
            return _where;
        }

        public virtual void CheckPropertieNameOrderBy()
        {
            Type typeObject = GetType();
            PropertyInfo[] properties = typeObject.GetProperties();
            bool ret = false;
            foreach (var item in properties)
                ret = !ret && item.Name == _orderBy ? true : ret;
        }
    }
}