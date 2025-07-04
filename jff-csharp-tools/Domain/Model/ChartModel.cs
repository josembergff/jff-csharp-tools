namespace JffCsharpTools.Domain.Model
{
    /// <summary>
    /// Represents a single data point for chart visualization.
    /// Contains a name-value pair commonly used in charts, graphs, and data visualization components.
    /// </summary>
    public class ChartModel
    {
        /// <summary>
        /// The label or name for this data point (e.g., "January", "Product A", "Category 1").
        /// Used as the identifier or axis label in chart representations.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The numeric value associated with this data point.
        /// Represents the measured quantity, count, percentage, or any numeric data for visualization.
        /// </summary>
        public decimal Value { get; set; } = 0;
    }
}