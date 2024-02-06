using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestODataRendering
        {
        [Test]
        public void Test1()
            {
            var p = new ProcessCommand("EntityPerson", "EntityPerson");
            var e = p.EditRecord(new IdentifyByPrimaryKey(15284));
            e.AddAliasedAttribute("Prefix", "Description", "Mr");
            e.AddAttribute("FirstName", "Jack");
            e.AddAttribute("NameFormat", "DEFAULT");
            var c = e.AddChild("Relate");
            var ec1 = c.EditRecord(new IdentifyByPosition(0));
            ec1.AddAttribute("Description", "New description");
            var addSite = ec1.AddChild("Site");
            var addSiteOp = addSite.AddRecord();
            addSiteOp.AddAttribute("SiteType", "BILLING");
            addSiteOp.AddAttribute("Description", "new site");
            var editSiteOp = addSite.EditRecord(new IdentifyByPrimaryKey(12345));
#if NET6_0_OR_GREATER
            editSiteOp.AddDateAttribute("FinishDate", DateOnly.FromDateTime(DateTime.Today));
#else
            editSiteOp.AddDateAttribute("FinishDate", DateTime.Today);
#endif
            editSiteOp.AddAttribute("IsDefault", false);

            var renderer = new ODataRenderer();
            var result = renderer.Render(p);
            Console.Write(result.Json);
            }
        }
    }
