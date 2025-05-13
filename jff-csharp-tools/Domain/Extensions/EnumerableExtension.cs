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
    public static class EnumerableExtension
    {
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