using System;
using System.Linq.Expressions;
using System.Reflection;

namespace JffCsharpTools.Domain.Filters
{
    public class DefaultFilter<TEntity>
    {
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

        public virtual Expression<Func<TEntity, bool>> Where() { return c => true; }

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