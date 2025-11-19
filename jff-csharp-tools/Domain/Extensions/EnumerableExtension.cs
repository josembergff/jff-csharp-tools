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
        /// Converts an enumerable collection to a CSV byte array
        /// Uses the specified culture for formatting   
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection</typeparam>
        /// <param name="exportList">The collection to convert to CSV</param>
        /// <param name="nameCulture">The name of the culture to use for formatting</param>
        /// <returns>A byte array containing the CSV data encoded in UTF-8</returns>
        public static byte[] ToCSV<T>(this IEnumerable<T> exportList, string nameCulture = "pt-BR")
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(new CultureInfo(nameCulture))))
            {
                csv.WriteRecords(exportList);
                writer.Flush();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Converts a list of dictionaries to a CSV string
        /// Each dictionary represents a row with key-value pairs as column names and values
        /// Uses the specified separator for columns
        /// If the list is empty or null, returns an empty string
        /// </summary>
        /// <param name="data">The list of dictionaries to convert to CSV</param>
        /// <param name="separator">The separator to use for columns</param>
        /// <returns>A CSV string representation of the data</returns>
        public static string ToCSV(List<Dictionary<string, object>> data, string separator = ";")
        {
            if (data == null || !data.Any())
                return string.Empty;

            var csv = new StringBuilder();

            // Cabeçalho
            var headers = data.First().Keys;
            csv.AppendLine(string.Join(separator, headers.Select(h => $"\"{h}\"")));

            // Dados
            foreach (var row in data)
            {
                var values = headers.Select(h =>
                {
                    var value = row.ContainsKey(h) ? row[h]?.ToString() ?? "" : "";
                    return $"\"{value}\"";
                });
                csv.AppendLine(string.Join(separator, values));
            }

            return csv.ToString();
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