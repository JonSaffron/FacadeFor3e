using System;
using System.Collections.Generic;
using FacadeFor3e.ProcessCommandBuilder;

namespace FacadeFor3e.Examples
    {
    public class OpenMatterExample
        {
        public static void OpenMatter(Dictionary<string, string> formData, bool isTender)
            {
            // first create a pending matter with the minimum amount of data required
            var newMatterIndex = OpenNewMatter.CreatePendingMatter(formData, isTender);

            // later, after collecting more input from the user, you can add missing details and change the matter status to Open
            string alternativeMatterNumber = formData["AlternativeMatterNumber"];
            bool useTimeType = bool.Parse(formData["UseTimeType"]);
            formData.TryGetValue("BillingGroup", out var billingGroup);
            formData.TryGetValue("phaseTaskActivityGroup", out var phaseTaskActivityGroup);
            OpenNewMatter.MatterOpen(newMatterIndex, alternativeMatterNumber, useTimeType, billingGroup, phaseTaskActivityGroup);
            }
        }

    static class OpenNewMatter
        {
        public static int CreatePendingMatter(Dictionary<string, string> fd, bool isTender)
            {
#if NET6_0_OR_GREATER
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
#else
            DateTime today = DateTime.Today;
#endif

            var p = new ProcessCommand("Matter_Srv", "Matter");
            var a = p.AddRecord();
            a.AddAttribute("IsAutoNumbering", true);
            a.AddDateAttribute("OpenDate", today);
            a.AddAttribute("MattStatus", "PE");     // pending status

            // MattDate child must come first. A new matter creates a MattDate record automatically.
            var md = a.AddChild("MattDate").EditRecord(new IdentifyByPosition(0));
            md.AddDateAttribute("EffStart", today);

            // MattRate must come after MattDate. A new matter creates a MattRate record automatically.
            var rate = a.AddChild("MattRate").EditRecord(new IdentifyByPosition(0));
            rate.AddAttribute("Rate", "HEADLINE");
            rate.AddAttribute("IsActive", true);

            // choose client
            a.AddAliasedAttribute("Client", "Number", fd["ClientNumber"]);
            a.AddAttribute("BillSite", fd["BillSite"]);

            // matter information
            string description = fd["MatterDescription"];
            a.AddAttribute("DisplayName", description);
            a.AddAttribute("Description", description);
            if (isTender)
                {
                md.AddAttribute("PracticeGroup", "NONBILL");
                a.AddAttribute("IsNonBillable", true);
                a.AddAttribute("NonBillType", "MARKETING");
                }
            else
                {
                string practiceGroup = fd["PracticeGroup"];
                md.AddAttribute("PracticeGroup", practiceGroup);
                }

            string partnerInitials = fd["Partner"];
            string executiveInitials = fd["Executive"];
            md.AddAliasedAttribute("BillTkpr", "Number", partnerInitials);
            md.AddAliasedAttribute("SpvTkpr", "Number", executiveInitials);
            md.AddAliasedAttribute("RspTkpr", "Number", executiveInitials);
            md.AddAttribute("Office", fd["Office"]);
            md.AddAttribute("Department", fd["Department"]);
            md.AddAttribute("Section", "000");  

            // financial arrangements
            if (isTender)
                {
                a.AddAttribute("Currency", "GBP");
                md.AddAttribute("Arrangement", "NCHG"); // no charge
                }
            else
                {
                string currency = fd["MatterCurrency"];
                string arrangement = fd["Arrangement"];
                string taxCode = fd["VatStatus"];
                a.AddAttribute("Currency", currency);
                md.AddAttribute("Arrangement", arrangement);
                a.AddAttribute("TimeTaxCode", taxCode);
                a.AddAttribute("CostTaxCode", taxCode);
                a.AddAttribute("ChrgTaxCode", taxCode);

                a.AddAttribute("BankAcctAp", fd["BankAccountAp"]);
                }

            a.AddAttribute("OpenTkpr", fd["OpenTkpr"]);

            int newMatterIndex = int.Parse(RunProcessIn3E(p));
            return newMatterIndex;
            }

	    public static void MatterOpen(int matter, string alternativeMatterNumber, bool useTimeType, string billingGroup, string ptaGroup = null)
            {
#if NET6_0_OR_GREATER
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
#else
            DateTime today = DateTime.Today;
#endif

            var p = new ProcessCommand("Matter_Srv", "Matter");
            var e = p.EditRecord(new IdentifyByPrimaryKey(new IntAttribute(matter)));
            e.AddAttribute("MattStatus", "OP");     // set status to open
            e.AddDateAttribute("OpenDate", today);     // reset open date
            e.AddAttribute("AltNumber", alternativeMatterNumber);

            if (!string.IsNullOrWhiteSpace(ptaGroup))
                {   // update PTA group
                var md = e.AddChild("MattDate").EditRecord(new IdentifyByPosition(0));
                md.AddAttribute("PTAGroup" , ptaGroup);
                }
            
            if (useTimeType)
                {   // use time type during time capture
                var c = e.AddChild("MattTimeType");
                var a = c.AddRecord();
                a.AddAttribute("Description", "Time and travel");
                a.AddAttribute("IsIncludeList", true);

                var d = a.AddChild("MattTimeTypeDet");
                var d1 = d.EditRecord(new IdentifyByPosition(0));  // when you add a MattTimeType, you get one detail row too
                d1.AddAttribute("TimeType", "FEES");
                d1.AddAttribute("IsDefault", true);
                
                var d2 = d.AddRecord();              // add a second detail row
                d2.AddAttribute("TimeType", "TRAVEL");
                d2.AddAttribute("IsDefault", false);
                }

            if (billingGroup != null)
                {   // set the billing group
                var c = e.AddChild("BillingGroupMatter1");
                var b = c.AddRecord();
                b.AddAttribute("BillingGroup", billingGroup);
                }

	        RunProcessIn3E(p);
            }

        private static string RunProcessIn3E(ProcessCommand p)
            {
            var endpoint = new Uri("http://wapi123.lawfirm/TE_3E_LIVE/web/ui/TransactionService.asmx");
            var result = ExecuteProcess.Execute(p, endpoint);
            return result;
            }
        }
    }
