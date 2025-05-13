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

        public static PaginationModel<T1, T3> CriarResultadoRespostaPaginado<T1, T2, T3>(this IEnumerable<T1> pagedResults, T2 paginacaoModel, bool paginacaoPendente = false) where T2 : PaginationModel<T1, T3> where T3 : DefaultFilter<T3>, new()
        {
            if (paginacaoModel != null)
            {
                paginacaoModel.List = pagedResults.ToList();
                paginacaoModel.Total = pagedResults.Count();

                if (paginacaoPendente)
                {
                    if (paginacaoModel.CheckOrder(paginacaoModel.List))
                    {
                        var listaOrdenada = paginacaoModel.List.AsQueryable().ApplyOrderBy(!paginacaoModel.OrderDescending, paginacaoModel.Order);
                        paginacaoModel.List = listaOrdenada.ToList();
                    }

                    var itens = paginacaoModel.List.ToList();

                    if (!paginacaoModel.IgnorePagination)
                    {
                        itens = paginacaoModel.List.Skip(paginacaoModel.SkipTotal)
                            .Take(paginacaoModel.CountPerPage).ToList();
                    }

                    paginacaoModel.List = itens;
                }
            }
            return paginacaoModel;
        }
    }
}