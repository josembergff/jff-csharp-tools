using System.Collections.Generic;

namespace JffCsharpTools.Domain.Model
{
    public class SerieChartModel
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<ChartModel> Series { get; set; } = new List<ChartModel>();
    }
}