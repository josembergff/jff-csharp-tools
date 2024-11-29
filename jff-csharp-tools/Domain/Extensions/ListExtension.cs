using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;

namespace JffCsharpTools.Dominio.Extensions
{
    public static class ListExtension
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
    }
}