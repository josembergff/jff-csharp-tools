using System.Collections.Generic;

namespace JffCsharpTools.Domain.Model
{
    /// <summary>
    /// Represents a chart series model containing a collection of chart data points.
    /// Used for organizing chart data with a descriptive name and associated data series.
    /// </summary>
    public class SerieChartModel
    {
        /// <summary>
        /// The name or title of the chart series (e.g., "Sales Data", "Temperature Readings")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of chart data points that belong to this series.
        /// Each ChartModel represents a single data point with coordinates or values.
        /// </summary>
        public IEnumerable<ChartModel> Series { get; set; } = new List<ChartModel>();
    }
}