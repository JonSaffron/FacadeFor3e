using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A collection of Operations
    /// </summary>
    [PublicAPI]
    public class OperationCollection : Collection<OperationBase>
        {
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
