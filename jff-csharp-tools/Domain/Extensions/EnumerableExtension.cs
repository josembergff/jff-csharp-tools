using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable to provide additional collection functionality
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Converts an enumerable collection to CSV format as byte array
        /// Uses Brazilian Portuguese culture for formatting
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection</typeparam>
        /// <param name="exportList">The collection to convert to CSV</param>
        /// <returns>A byte array containing the CSV data encoded in UTF-8</returns>
        public static byte[] ToCSV<T>(this IEnumerable<T> exportList)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(new CultureInfo("pt-BR"))))
            {
                csv.WriteRecords(exportList);
                writer.Flush();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Creates a paginated response result from an enumerable collection
        /// Handles ordering, pagination, and total count calculation
        /// </summary>
        /// <typeparam name="T1">The type of items in the collection</typeparam>
        /// <typeparam name="T2">The type of pagination model that inherits from PaginationModel</typeparam>
        /// <param name="pagedResults">The collection of results to paginate</param>
        /// <param name="paginationModel">The pagination model containing pagination parameters</param>
        /// <param name="pendingPagination">Whether pagination should be applied (default: false)</param>
        /// <returns>A PaginationModel containing the paginated results and metadata</returns>
        public static PaginationModel<T1> CreatePaginatedResponseResult<T1, T2>(this IEnumerable<T1> pagedResults, T2 paginationModel, bool pendingPagination = false) where T2 : PaginationModel<T1>
        {
            if (paginationModel != null)
            {
                paginationModel.List = pagedResults.ToList();
                paginationModel.Total = pagedResults.Count();

                if (pendingPagination)
                {
                    if (paginationModel.CheckOrder(paginationModel.List))
                    {
                        var orderedList = paginationModel.List.AsQueryable().ApplyOrderBy(!paginationModel.OrderDescending, paginationModel.Order);
                        paginationModel.List = orderedList.ToList();
                    }

                    var items = paginationModel.List.ToList();

                    if (!paginationModel.IgnorePagination)
                    {
                        items = paginationModel.List.Skip(paginationModel.SkipTotal)
                            .Take(paginationModel.CountPerPage).ToList();
                    }

                    paginationModel.List = items;
                }
            }
            return paginationModel;
        }
    }
}