using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A collection of Operations
    /// </summary>
    [PublicAPI]
    public class OperationCollection : Collection<OperationBase>
        {
        /// <summary>
        /// Adds a collection of Operation objects
        /// </summary>
        /// <param name="items">The collection of Operations to add</param>
        /// <remarks>None of the items to be added can be null</remarks>
        public void AddRange(IEnumerable<OperationBase> items)
            {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            var itemList = items.ToList();
            // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (itemList.Any(item => item == null))
                // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                throw new ArgumentException("Cannot add a null value to the collection.");
            itemList.ForEach(this.Add);
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void InsertItem(int index, OperationBase item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (this.Contains(item))
                throw new ArgumentOutOfRangeException(nameof(item), "Operation has already been added.");
            base.InsertItem(index, item);
            }

        /// <inheritdoc />
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void SetItem(int index, OperationBase item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            int existingIndex = this.IndexOf(item);
            if (existingIndex != -1)
                {
                if (existingIndex != index)
                    {
                    throw new ArgumentOutOfRangeException(nameof(item), "Operation is already present in the collection.");
                    }
                return;
                }
            base.SetItem(index, item);
            }
        }
    }
