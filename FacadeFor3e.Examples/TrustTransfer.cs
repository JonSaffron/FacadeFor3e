using System;
using System.Linq;
using System.Net;
using FacadeFor3e.ProcessCommandBuilder;

namespace FacadeFor3e.Examples
    {
    class RunTrustTransfer
        {
        public static void TrustTransferExample()
            {
            const string matterNumber = "A1234-98765";

            var ttp = new TrustTransfer.TrustTransferParameters
                {
                TrustTransferType = "USES",
                AuthorisedBy = "Joe Bloggs",
                DocumentNumber = "1025",
                Narrative = "Transfer from deposit to current account",

                FromMatter = matterNumber,
                FromBankAccount = "GBPDEP1",
                FromIntendedUse = "Deposit",
                FromIntendedUseInfo = 1234,
                FromIntendedUseDetails = null,
                Amount = 1000,

                ToMatter = matterNumber,
                ToBankAccount = "GBPCUR1",
                ToIntendedUse = "GENERAL"
                };

            var tt = new TrustTransfer();
            int trustTransferIndex = tt.PerformTrustTransfer(ttp);
            Console.WriteLine("New TrustTransfer Index: {0}", trustTransferIndex);
            }
        }

    class TrustTransfer 
        {
        private Uri endpoint = new Uri("http://myserver/TE_3E_xxx/WebUi/TransactionService.asmx");
        private NetworkCredential credentials = new NetworkCredential("user", "password", "domain");

        public int PerformTrustTransfer(TrustTransferParameters ttp)
            {
            var p = new ProcessCommand("N_WsTrustTransfer", "TrustTransMaster");
            var a = p.AddRecord();
            a.AddAttribute("TrustTransferType", ttp.TrustTransferType);
            a.AddAttribute("AuthorizedBy", ttp.AuthorisedBy);
            a.AddAttribute("DocumentNumber", ttp.DocumentNumber);
            a.AddAttribute("Narrative", ttp.Narrative);

            var fromChild = a.AddChild("TrustTransferDetFrom");
            var from = fromChild.EditRecord(new IdentifyByPosition(0));
            from.AddAliasedAttribute("Matter", "Number", ttp.FromMatter);
            from.AddAliasedAttribute("BankAcctTrust", "Name", ttp.FromBankAccount);
            from.AddAttribute("TrustIntendedUse", ttp.FromIntendedUse);
            from.AddAttribute("TrustIntendedUseInfo", ttp.FromIntendedUseInfo);
            from.AddAttribute("TrustIntendedUseDet", ttp.FromIntendedUseDetails);
            from.AddAttribute("Amount", ttp.Amount);

            var toChild = a.AddChild("TrustTransferDetTo");
            var to = toChild.EditRecord(new IdentifyByPosition(0));
            to.AddAliasedAttribute("Matter", "Number", ttp.ToMatter);
            to.AddAliasedAttribute("BankAcctTrust", "Name", ttp.ToBankAccount);
            to.AddAttribute("TrustIntendedUse", ttp.ToIntendedUse);

            using (var rp = new TransactionServices(endpoint, credentials))
                {
                var ep = rp.ExecuteProcess;
                ep.GetKeys = true;
                ep.ThrowExceptionIfProcessDoesNotComplete = true;
                ep.ThrowExceptionIfDataErrorsFound = true;
 
                var r = ep.Execute(p);
                int result = int.Parse(r.GetKeys().First());
                return result;
                }
            }

        public class TrustTransferParameters
            {
            public string TrustTransferType;
            public string AuthorisedBy;
            public string DocumentNumber;
            public string Narrative;

            public string FromMatter;
            public string FromBankAccount;
            public string FromIntendedUse;
            public int? FromIntendedUseInfo;
            public string FromIntendedUseDetails;
            public decimal Amount;

            public string ToMatter;
            public string ToBankAccount;
            public string ToIntendedUse;
            }
        }
    }
