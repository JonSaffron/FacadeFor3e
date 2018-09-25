using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;

namespace FacadeFor3e.Examples
    {
    static class OpenNewMatter
        {
        public static int CreatePendingMatter(Dictionary<string, string> fd, bool isTender)
            {
            DateTime today = DateTime.Today;

            var p = new Process("Matter_Srv", "Matter");
            var a = p.AddOperation();
            a.AddAttribute("IsAutoNumbering", true);
            a.AddAttribute("OpenDate", today);
            a.AddAttribute("MattStatus", "PE");     // pending status

            // MattDate child must come first. A new matter creates a MattDate record automatically.
            var md = a.AddChild("MattDate").EditOperationByPosition(0);
            md.AddAttribute("EffStart", today);

            // MattRate must come after MattDate. A new matter creates a MattRate record automatically.
            var rate = a.AddChild("MattRate").EditOperationByPosition(0);
            rate.AddAttribute("Rate", "HEADLINE");
            rate.AddAttribute("IsActive", true);

            // choose client
            a.AddAttribute("Client", "Number", fd["ClientNumber"]);
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
            md.AddAttribute("BillTkpr", "Number", partnerInitials);
            md.AddAttribute("SpvTkpr", "Number", executiveInitials);
            md.AddAttribute("RspTkpr", "Number", executiveInitials);
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
            var p = new Process("Matter_Srv", "Matter");
            var e = p.EditOperation(matter);
            e.AddAttribute("MattStatus", "OP");     // set status to open
            e.AddAttribute("OpenDate", DateTime.Today);     // reset open date
            e.AddAttribute("AltNumber", alternativeMatterNumber);

            if (!string.IsNullOrWhiteSpace(ptaGroup))
                {   // update PTA group
                var md = e.AddChild("MattDate").EditOperationByPosition(0);
                md.AddAttribute("PTAGroup" , ptaGroup);
                }
            
            if (useTimeType)
                {   // use time type during time capture
                var c = e.AddChild("MattTimeType");
                var a = c.AddOperation();
                a.AddAttribute("Description", "Time and travel");
                a.AddAttribute("IsIncludeList", true);

                var d = a.AddChild("MattTimeTypeDet");
                var d1 = d.EditOperationByPosition(0);  // when you add a MattTimeType, you get one detail row too
                d1.AddAttribute("TimeType", "FEES");
                d1.AddAttribute("IsDefault", true);
                
                var d2 = d.AddOperation();              // add a second detail row
                d2.AddAttribute("TimeType", "TRAVEL");
                d2.AddAttribute("IsDefault", false);
                }

            if (billingGroup != null)
                {   // set the billing group
                var c = e.AddChild("BillingGroupMatter1");
                var b = c.AddOperation();
                b.AddAttribute("BillingGroup", billingGroup);
                }

	        RunProcessIn3E(p);
            }

        private static string RunProcessIn3E(Process p)
            {
            // get the identity of the person using your website (this works when using windows authentication)
            var wi = (WindowsIdentity) HttpContext.Current.User.Identity;
#if DEBUG
            string result = RunProcess.ExecuteProcess(p, wi, "3eTransactionServiceDev");
#else
            string result = RunProcess.ExecuteProcess(p, wi, "3eTransactionServiceLive");
#endif
            return result;
            }
        }
    }
