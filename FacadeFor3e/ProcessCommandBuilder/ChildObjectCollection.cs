using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A collection of DataObject objects that are children of another DataObject
    /// </summary>
    [PublicAPI]
    public class ChildObjectCollection : Collection<DataObject>
        {
        /// <summary>
        /// Gets the DataObject with the specified name
        /// </summary>
        /// <param name="name">The name of the DataObject to retrieve</param>
        /// <returns>The DataObject with the specified name. If the specified name is not found, a <exception cref="KeyNotFoundException">KeyNotFoundException</exception> is thrown.</returns>
        public DataObject this[string name]
            {
            get
                {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                // ReSharper disable once PossibleNullReferenceException
                var result = this.SingleOrDefault(a => a.ObjectName.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (result == null)
                    throw new KeyNotFoundException("A child object could not be found with the specified name.");
                return result;
                }
            }

        /// <summary>
        /// Gets the DataObject with the specified name
        /// </summary>
        /// <param name="name">The name of the DataObject to retrieve</param>
        /// <param name="dataObject">When the method returns, contains the DataObject with the specified name if the name is found, otherwise null.</param>
        /// <returns>True if the DataObject specified was found, otherwise false.</returns>
        public bool TryGetValue(string name, out DataObject? dataObject)
            {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            // ReSharper disable once PossibleNullReferenceException
            dataObject = this.SingleOrDefault(a => a.ObjectName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return dataObject != null;
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void InsertItem(int index, DataObject item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            // ReSharper disable once PossibleNullReferenceException
            if (this.Any(a => a.ObjectName.Equals(item.ObjectName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException("An attribute with the name " + item.ObjectName + " has already been added.");
            base.InsertItem(index, item);
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void SetItem(int index, DataObject item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            // ReSharper disable once PossibleNullReferenceException
            if (this.Any(a => a.ObjectName.Equals(item.ObjectName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException("An attribute with the name " + item.ObjectName + " has already been added.");
            base.SetItem(index, item);
            }
        }
    }
