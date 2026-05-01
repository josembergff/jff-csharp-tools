using System.Collections.Generic;

namespace JffCsharpTools.Application.DTOs
{
    /// <summary>
    /// Represents a chart series DTO containing a collection of chart data points.
    /// Used for organizing chart data with a descriptive name and associated data series.
    /// </summary>
    public class SerieChartDto
    {
        /// <summary>
        /// The name or title of the chart series (e.g., "Sales Data", "Temperature Readings")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of chart data points that belong to this series.
        /// Each ChartDto represents a single data point with coordinates or values.
        /// </summary>
        public IEnumerable<ChartDto> Series { get; set; } = new List<ChartDto>();
    }
}