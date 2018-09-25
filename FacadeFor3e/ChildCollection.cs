using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FacadeFor3e
    {
    class ChildObjectCollection : Collection<DataObject>
        {
        protected override void InsertItem(int index, DataObject item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (this.Any(a => a.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException("An attribute with the name " + item.Name + " has already been added.");
            base.InsertItem(index, item);
            }

        protected override void SetItem(int index, DataObject item)
            {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (this.Any(a => a.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentOutOfRangeException("An attribute with the name " + item.Name + " has already been added.");
            base.SetItem(index, item);
            }
        }
    }
