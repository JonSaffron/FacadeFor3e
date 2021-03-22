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

            p.Add();

            p.Edit(new IdentifyByPosition(0));
            p.Edit(new IdentifyByPrimaryKey<DecimalAttribute>(10m));
            p.Edit(new IdentifyByPrimaryKey<IntAttribute>(99));
            p.Edit(new IdentifyByPrimaryKey<StringAttribute>("hello"));
            p.Edit(new IdentifyByPrimaryKey<GuidAttribute>(Guid.NewGuid()));
            p.Edit(new IdentifyByPrimaryKey<DateAttribute>(DateTime.Today));
            p.Edit(new IdentifyByPrimaryKey<DateTimeAttribute>(DateTime.Now));

            p.Delete(new IdentifyByPosition(0));
            p.Delete(new IdentifyByPrimaryKey<DecimalAttribute>(10m));
            p.Delete(new IdentifyByPrimaryKey<IntAttribute>(99));
            p.Delete(new IdentifyByPrimaryKey<StringAttribute>("hello"));
            p.Delete(new IdentifyByPrimaryKey<GuidAttribute>(Guid.NewGuid()));
            p.Delete(new IdentifyByPrimaryKey<DateAttribute>(DateTime.Today));
            p.Delete(new IdentifyByPrimaryKey<DateTimeAttribute>(DateTime.Now));


            p.Edit(new IdentifyByPrimaryKey<IntAttribute>(5));
            }

        }
    }
