using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using JffCsharpTools.Application.Common;
using JffCsharpTools.Domain.Exceptions;
using JffCsharpTools.Domain.Filters;

namespace JffCsharpTools.Domain.Common
{
    /// <summary>
    /// Generic pagination model for handling paginated data operations.
    /// Provides pagination controls, result data, and operation status information.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity being paginated</typeparam>
    public class PaginationResult<TEntity> : Result
    {
        /// <summary>
        /// Default constructor that initializes CountPerPage to 10 and Page to 1
        /// </summary>
        public PaginationResult()
        {
            CountPerPage = 10;
            Page = 1;
        }

        /// <summary>
        /// Constructor that initializes pagination settings from a filter object
        /// </summary>
        /// <param name="filter">Filter containing pagination parameters</param>
        public PaginationResult(DefaultFilter<TEntity> filter)
        {
            CountPerPage = filter.Count;
            Page = filter.Page;
        }


        public PaginationResult(int page, int countPerPage)
        {
            CountPerPage = countPerPage;
            Page = page;
        }

        /// <summary>
        /// Constructor that initializes pagination settings with ordering information
        /// </summary>
        /// <param name="page">Current page number (1-based indexing)</param>
        /// <param name="countPerPage">Number of items to display per page</param>
        /// <param name="order">Name of the property to order results by</param>
        /// <param name="orderDescending">Indicates whether to order results in descending order (true) or ascending order (false)</param>
        public PaginationResult(int page, int countPerPage, string order, bool orderDescending)
        {
            CountPerPage = countPerPage;
            Page = page;
            Order = order;
            OrderDescending = orderDescending;
        }

        /// <summary>
        /// Constructor that initializes the pagination result with domain exception information
        /// </summary>
        /// <param name="domainException">Domain exception that caused the pagination result to fail</param>
        public PaginationResult(DomainException domainException) : base(domainException)
        {

        }

        /// <summary>
        /// Constructor that initializes the pagination result with exception information
        /// </summary>
        /// <param name="exception">Exception that caused the pagination result to fail</param>
        public PaginationResult(Exception exception) : base(exception)
        {

        }

        /// <summary>
        /// Constructor that initializes the pagination result with error information
        /// </summary>
        /// <param name="error">Error code or identifier</param>
        /// <param name="errorMessage">Detailed error message</param>
        /// <param name="statusCode">HTTP status code associated with the error</param>
        public PaginationResult(string error, string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(error, errorMessage, statusCode)
        {

        }

        /// <summary>
        /// Current page number (1-based indexing)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items to display per page
        /// </summary>
        public int CountPerPage { get; set; }

        /// <summary>
        /// Name of the property to order results by
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Indicates whether to order results in descending order (true) or ascending order (false)
        /// </summary>
        public bool OrderDescending { get; set; }

        /// <summary>
        /// Total number of items available across all pages
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Collection of entities for the current page
        /// </summary>
        public IEnumerable<TEntity> List { get; set; } = Enumerable.Empty<TEntity>();

        /// <summary>
        /// Flag to disable pagination and return all results
        /// </summary>
        public bool IgnorePagination { get; set; } = false;

        /// <summary>
        /// Calculates the number of items to skip based on current page and items per page.
        /// Returns 0 if pagination is ignored, otherwise returns (Page - 1) * CountPerPage.
        /// </summary>
        public int SkipTotal
        {
            get { return IgnorePagination ? 0 : (Page - 1) * CountPerPage; }
        }

        /// <summary>
        /// Calculates the total number of pages based on Total items and CountPerPage.
        /// Returns 0 if CountPerPage or Total is 0 or negative, otherwise returns the ceiling of Total/CountPerPage.
        /// </summary>
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

        /// <summary>
        /// Validates if the specified Order property exists in the given list of objects.
        /// Uses reflection to check if any property in the object type matches the Order field.
        /// </summary>
        /// <typeparam name="T1">Type of objects in the list to validate against</typeparam>
        /// <param name="list">Collection of objects to validate the order property against</param>
        /// <returns>True if the Order property exists in the object type, false otherwise</returns>
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