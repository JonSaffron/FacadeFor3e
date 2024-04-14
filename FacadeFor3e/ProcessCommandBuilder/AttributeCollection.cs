using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A collection of <see cref="NamedAttributeValue"/> objects
    /// </summary>
    [PublicAPI]
    public class AttributeCollection : Collection<NamedAttributeValue>
        {
        /// <summary>
        /// Gets the attribute with the specified name
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve</param>
        /// <returns>The attribute with the specified name. If the specified name is not found, a <exception cref="KeyNotFoundException">KeyNotFoundException</exception> is thrown.</returns>
        public NamedAttributeValue this[string name]
            {
            get
                {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                // ReSharper disable once PossibleNullReferenceException
                var result = this.SingleOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (result == null)
                    throw new KeyNotFoundException("An attribute could not be found with the specified name.");
                return result;
                }
            }

        /// <summary>
        /// Gets the attribute with the specified name
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve</param>
        /// <param name="namedAttribute">When the method returns, contains the attribute with the specified name if the name is found, otherwise null.</param>
        /// <returns>True if the attribute specified was found, otherwise false.</returns>
        public bool TryGetValue(string name, out NamedAttributeValue? namedAttribute)
            {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            // ReSharper disable once PossibleNullReferenceException
            namedAttribute = this.SingleOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return namedAttribute != null;
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void InsertItem(int index, NamedAttributeValue item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            // ReSharper disable once PossibleNullReferenceException
            if (this.Any(a => a.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException($"An attribute with the name {item.Name} has already been added.");
            base.InsertItem(index, item);
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void SetItem(int index, NamedAttributeValue item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            for (int i = 0; i < this.Count; i++)
                {
                if (i == index)
                    continue;

                var a = this[i];
                if (a == null)
                    throw new InvalidOperationException();
                if (a.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentOutOfRangeException($"An attribute with the name {item.Name} already exists.");
                }

            base.SetItem(index, item);
            }

        /// <summary>
        /// Removes the specified attribute from the collection
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve</param>
        /// <returns>True if the specified attribute was found, false otherwise</returns>
        /// <exception cref="ArgumentNullException">If a null value for the attribute name is specified</exception>
        public bool Remove(string name)
            {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            for (int i = 0; i < this.Count; i++)
                {
                var a = this[i];

                if (a == null)
                    throw new InvalidOperationException();
                if (a.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                    // ReSharper disable once RedundantBaseQualifier
                    base.RemoveAt(i);
                    return true;
                    }
                }

            return false;
            }
        }
    }
