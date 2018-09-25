using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FacadeFor3e.Examples
    {
    class TrustTransfer : IDisposable
        {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LogonUser(string principal, string authority, string password, LogonSessionType logonType, LogonProvider logonProvider, out IntPtr token);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        static extern bool DuplicateToken(IntPtr hToken, SecurityImpersonationLevel impersonationLevel,  ref IntPtr hNewToken);

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        enum LogonSessionType : uint
            {
            Interactive = 2,
            Network = 3,
            Batch = 4,
            Service = 5,
            Unlock = 7,
            NetworkCleartext = 8,
            NewCredentials = 9,
            RemoteInteractive = 10,
            CachedInteractive = 11
            }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        enum LogonProvider : uint
            {
            Default = 0, // default for platform (use this!)
            WinNT35,     // sends smoke signals to authority
            WinNT40,     // uses NTLM
            WinNT50      // negotiates Kerb or NTLM
            }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        enum SecurityImpersonationLevel : uint
            {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
            }
        
        IntPtr _originalToken = IntPtr.Zero;
        IntPtr _duplicatedToken = IntPtr.Zero;
        private WindowsIdentity _wi;
        private bool _isDisposed;

        private static readonly string EndpointName;

        static TrustTransfer()
            {
#if DEBUG
            EndpointName = "3eTransactionServiceDev";
#else
            EndpointName = "3eTransactionServiceLive";
#endif
            }

        private WindowsIdentity GetWindowsIdentity()
            {
            if (this._wi == null)
                {
                string username = ConfigurationManager.AppSettings["3eUserName"];
                string domain = ConfigurationManager.AppSettings["3eDomain"];
                string password = ConfigurationManager.AppSettings["3ePassword"];

                bool result = LogonUser(username, domain, password, LogonSessionType.Interactive, LogonProvider.Default, out _originalToken);
                if (!result)
                    {
                    System.Diagnostics.Trace.WriteLine("LogonUser failed.");
                    throw new Win32Exception();
                    }

                result = DuplicateToken(_originalToken, SecurityImpersonationLevel.SecurityImpersonation, ref _duplicatedToken);
                if (!result)
                    {
                    System.Diagnostics.Trace.WriteLine("DuplicateToken failed.");
                    throw new Win32Exception();
                    }

                this._wi = new WindowsIdentity(_duplicatedToken);
                }

            return this._wi;
            }

        public int PerformTrustTransfer(TrustTransferParameters ttp)
            {
            if (this._isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            var p = new Process("N_WsTrustTransfer", "TrustTransMaster");
            var a = p.AddOperation();
            a.AddAttribute("TrustTransferType", ttp.TrustTransferType);
            a.AddAttribute("AuthorizedBy", ttp.AuthorisedBy);
            a.AddAttribute("DocumentNumber", ttp.DocumentNumber);
            a.AddAttribute("Narrative", ttp.Narrative);

            var fromChild = a.AddChild("TrustTransferDetFrom");
            var from = fromChild.EditOperationByPosition(0);
            from.AddAttribute("Matter", "Number", ttp.FromMatter);
            from.AddAttribute("BankAcctTrust", "Name", ttp.FromBankAccount);
            from.AddAttribute("TrustIntendedUse", ttp.FromIntendedUse);
            from.AddAttribute("TrustIntendedUseInfo", ttp.FromIntendedUseInfo);
            from.AddAttribute("TrustIntendedUseDet", ttp.FromIntendedUseDetails);
            from.AddAttribute("Amount", ttp.Amount);

            var toChild = a.AddChild("TrustTransferDetTo");
            var to = toChild.EditOperationByPosition(0);
            to.AddAttribute("Matter", "Number", ttp.ToMatter);
            to.AddAttribute("BankAcctTrust", "Name", ttp.ToBankAccount);
            to.AddAttribute("TrustIntendedUse", ttp.ToIntendedUse);

            using (var rp = new RunProcess())
                {
                rp.AccountToImpersonate = GetWindowsIdentity();
                rp.EndpointName = EndpointName;
                rp.GetKeys = true;
                rp.ThrowExceptionIfProcessDoesNotComplete = true;
 
                var r = rp.Execute(p);
                int result = int.Parse(r.GetKeys().First());
                return result;
                }
            }

        public void Dispose()
            {
            if (this._wi != null)
                this._wi.Dispose();

            if (this._duplicatedToken != IntPtr.Zero)
                CloseHandle(_duplicatedToken);

            if (this._originalToken != IntPtr.Zero)
                CloseHandle(_originalToken);

            this._wi = null;
            this._duplicatedToken = IntPtr.Zero;
            this._originalToken = IntPtr.Zero;
            this._isDisposed = true;
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
