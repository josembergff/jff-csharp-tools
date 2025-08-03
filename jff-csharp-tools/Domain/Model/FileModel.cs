
using JffCsharpTools.Domain.Enums;

namespace JffCsharpTools.Domain.Model
{
    /// <summary>
    /// Represents a file model containing file data, metadata and type information.
    /// This class encapsulates all the necessary properties to handle file operations.
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// Private backing field for the TypeContentFile property.
        /// Stores the nullable enum value for file content type.
        /// </summary>
        private TypeContentFileEnum? _typeContentFile;

        /// <summary>
        /// Gets or sets the binary data of the file as a byte array.
        /// Initialized with an empty byte array to prevent null reference exceptions.
        /// </summary>
        public byte[] File { get; set; } = new byte[0];

        /// <summary>
        /// Gets or sets the name of the file.
        /// Initialized with an empty string to prevent null reference exceptions.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the content type of the file.
        /// Returns UNKNOWN if the backing field is null, otherwise returns the stored value.
        /// </summary>
        public TypeContentFileEnum TypeContentFile
        {
            get { return _typeContentFile != null ? _typeContentFile.Value : TypeContentFileEnum.UNKNOWN; }
            set { _typeContentFile = value; }
        }

        /// <summary>
        /// Gets the file extension based on the TypeContentFile enum value.
        /// Returns the enum value converted to lowercase string format.
        /// </summary>
        public string Extension { get => TypeContentFile.ToString().ToLower(); }
    }
}