using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// An attribute with a value from an alias
    /// </summary>
    [PublicAPI]
    public sealed class AliasAttribute : NamedAttributeValue
        {
        /// <summary>
        /// The alias attribute name
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Constructs a new AliasAttribute object
        /// </summary>
        /// <param name="name">The name to give the attribute</param>
        /// <param name="alias">The name of the attribute that contains the value to search for</param>
        /// <param name="attribute">The value within the collection to search for</param>
        public AliasAttribute(string name, string alias, IAttribute attribute) : base(name, attribute)
            {
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            CommonLibrary.EnsureValid(alias);
            }
        }
    }
