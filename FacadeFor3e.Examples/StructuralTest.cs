using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacadeFor3e.Examples
    {
    class StructuralTest
        {
        private static void AllOperations()
            {
            var p = new Process("process", "object");

            p.AddOperation();
            p.AddOperationFromExistingRow(Guid.NewGuid());
            p.AddOperationFromModel(Guid.NewGuid());

            p.EditOperation(new IdentifyByPosition(0));
            p.EditOperation(new IdentifyByPrimaryKey<DecimalAttribute>(10m));
            p.EditOperation(new IdentifyByPrimaryKey<IntAttribute>(99));
            p.EditOperation(new IdentifyByPrimaryKey<StringAttribute>("hello"));
            p.EditOperation(new IdentifyByPrimaryKey<GuidAttribute>(Guid.NewGuid()));
            p.EditOperation(new IdentifyByPrimaryKey<DateAttribute>(DateTime.Today));
            p.EditOperation(new IdentifyByPrimaryKey<DateTimeAttribute>(DateTime.Now));

            p.DeleteOperation(new IdentifyByPosition(0));
            p.DeleteOperation(new IdentifyByPrimaryKey<DecimalAttribute>(10m));
            p.DeleteOperation(new IdentifyByPrimaryKey<IntAttribute>(99));
            p.DeleteOperation(new IdentifyByPrimaryKey<StringAttribute>("hello"));
            p.DeleteOperation(new IdentifyByPrimaryKey<GuidAttribute>(Guid.NewGuid()));
            p.DeleteOperation(new IdentifyByPrimaryKey<DateAttribute>(DateTime.Today));
            p.DeleteOperation(new IdentifyByPrimaryKey<DateTimeAttribute>(DateTime.Now));


            p.EditOperation(IdentifyByPrimaryKey<IntAttribute>.From(5m));
            }

        }
    }
