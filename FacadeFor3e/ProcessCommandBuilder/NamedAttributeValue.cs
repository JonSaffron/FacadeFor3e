using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A named attribute value
    /// </summary>
    [PublicAPI]
    public class NamedAttributeValue
        {
        /// <summary>
        /// The attribute name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The attribute value
        /// </summary>
        public IAttribute Attribute { get; }

        /// <summary>
        /// Constructs a new NamedAttributeValue object
        /// </summary>
        /// <param name="name">The name to give the attribute</param>
        /// <param name="attribute">The value to give the attribute</param>
        public NamedAttributeValue(string name, IAttribute attribute)
            {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            CommonLibrary.EnsureValid(name);
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            }
        }
    }
