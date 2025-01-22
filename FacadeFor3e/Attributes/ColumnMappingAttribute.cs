using System;

namespace FacadeFor3e
    {
    /// <summary>
    /// Describes which column in the returned data that the property/field will get its data from
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ColumnMappingAttribute : Attribute
        {
        /// <summary>
        /// Returns the name of the column that data to retrieve
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructs a mapping to a specific column in the returned data
        /// </summary>
        /// <param name="name">Specifies which column to map to</param>
        /// <exception cref="ArgumentNullException">Raised if the passed in name is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raise if the passed in name is empty or whitespace</exception>
        public ColumnMappingAttribute(string name)
            {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Name of the column cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Name of column is invalid");
            this.Name = name;
            }
        }
    }
