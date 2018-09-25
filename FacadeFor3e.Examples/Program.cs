using System.Collections.Generic;

namespace FacadeFor3e.Examples
    {
    static class Program
        {
        static void Main()
            {
            }

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

            using (var tt = new TrustTransfer())
                {
                // ReSharper disable once UnusedVariable
                int trustTransferIndex = tt.PerformTrustTransfer(ttp);
                }
            }

        public static void OpenMatterExample(Dictionary<string, string> formData, bool isTender)
            {
            var newMatterIndex = OpenNewMatter.CreatePendingMatter(formData, isTender);

            // any other processing

            string alternativeMatterNumber = formData["AlternativeMatterNumber"];
            bool useTimeType = bool.Parse(formData["UseTimeType"]);
            string billingGroup;
            formData.TryGetValue("BillingGroup", out billingGroup);
            string phaseTaskActivityGroup;
            formData.TryGetValue("phaseTaskActivityGroup", out phaseTaskActivityGroup);

            OpenNewMatter.MatterOpen(newMatterIndex, alternativeMatterNumber, useTimeType, billingGroup, phaseTaskActivityGroup);
            }
        }
    }
