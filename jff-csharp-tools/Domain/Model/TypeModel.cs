namespace JffCsharpTools.Domain.Model
{
    /// <summary>
    /// Represents a type model entity that can be used to categorize or classify items.
    /// This class provides basic properties for identification, naming, and selection state.
    /// </summary>
    public class TypeModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the type.
        /// Default value is 0, typically used for new instances before database assignment.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Gets or sets the name of the type.
        /// This property represents the display name or title of the type.
        /// Default value is an empty string to avoid null reference issues.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the type.
        /// Provides additional details or explanation about what this type represents.
        /// Default value is an empty string to avoid null reference issues.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this type is currently selected.
        /// Useful for UI scenarios where users can select/deselect types from a list.
        /// Default value is false (not selected).
        /// </summary>
        public bool Selected { get; set; }
    }
}