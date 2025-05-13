using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;

namespace JffCsharpTools.Domain.Model
{
    public class PaginationModel<TEntity, TFilter> where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
    {
        public PaginationModel()
        {
            CountPerPage = 10;
            Page = 1;
        }
        public PaginationModel(DefaultFilter<TEntity> filter)
        {
            CountPerPage = filter.Count;
            Page = filter.Page;
        }
        private bool _success = true;
        public int Page { get; set; }
        public int CountPerPage { get; set; }
        public string Order { get; set; }
        public bool OrderDescending { get; set; }
        public int Total { get; set; }
        public TFilter Filter { get; set; }
        public IEnumerable<TEntity> List { get; set; } = Enumerable.Empty<TEntity>();
        public bool IgnorePagination = false;
        public bool Success
        {
            get
            {
                return _success && (ErrorList == null || !ErrorList.Any());
            }
            set
            {
                _success = value;
            }
        }
        public ICollection<string> ErrorList { get; set; }
        public ICollection<string> SuccessList { get; set; }
        public int SkipTotal
        {
            get { return IgnorePagination ? 0 : (Page - 1) * CountPerPage; }
        }

        public int TotalPages
        {
            get
            {
                var result = 0;

                if (CountPerPage > 0 && Total > 0)
                {
                    var division = (double)Total / (double)CountPerPage;
                    result = (int)Math.Ceiling(division);
                }
                return result;
            }
        }
        public virtual bool CheckOrder<T1>(IEnumerable<T1> list)
        {
            var result = false;
            if (list?.Any() == true && !string.IsNullOrEmpty(Order))
            {
                Type typeObject = list.FirstOrDefault().GetType();
                PropertyInfo[] properties = typeObject.GetProperties();
                bool ret = false;
                foreach (var item in properties)
                    ret = !ret && item.Name == Order ? true : ret;
                result = ret;
            }
            return result;
        }
    }
}