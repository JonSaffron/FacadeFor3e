using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;

// Example of using SafeHandles with the Windows API taken from
// https://github.com/mj1856/SimpleImpersonation 

namespace FacadeFor3e
    {
    /// <summary>
    /// Creates a WindowsIdentity object that can be used for impersonation
    /// </summary>
    [PublicAPI]
    public sealed class ProxyIdentityProvider : IDisposable
        {
        private readonly string _username;
        private readonly string _domain;
        private readonly string _password;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool LogonUser(string principal, string authority, string password, LogonSessionType logonType, LogonProvider logonProvider, out SafeAccessTokenHandle token);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool DuplicateToken(IntPtr hToken, SecurityImpersonationLevel impersonationLevel, out SafeAccessTokenHandle hNewToken);

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum LogonSessionType : uint
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

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        // ReSharper disable InconsistentNaming
        private enum LogonProvider : uint
            {
            Default = 0, // default for platform (use this!)
            WinNT35, // sends smoke signals to authority
            WinNT40, // uses NTLM
            WinNT50 // negotiates Kerberos or NTLM
            }
        // ReSharper restore InconsistentNaming

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private enum SecurityImpersonationLevel : uint
            {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
            }

        private SafeAccessTokenHandle? _originalToken;
        private SafeAccessTokenHandle? _duplicatedToken;
        private WindowsIdentity? _wi;

        /// <summary>
        /// Constructs a new ProxyIdentityProvider with the specified credentials
        /// </summary>
        /// <param name="username">Specifies the username of the account</param>
        /// <param name="domain">Specifies the domain where the account is defined</param>
        /// <param name="password">Specifies the password for the account</param>
        public ProxyIdentityProvider(string username, string domain, string password)
            {
            this._username = username ?? throw new ArgumentNullException(nameof(username));
            this._domain = domain ?? throw new ArgumentNullException(nameof(domain));
            this._password = password ?? throw new ArgumentNullException(nameof(password));
            }

        /// <summary>
        /// Gets the WindowsIdentity for the specified credentials
        /// </summary>
        /// <returns>A WindowsIdentity object that can be used for impersonation if the credentials are valid. A Win32Exception will be thrown otherwise.</returns>
        public WindowsIdentity GetWindowsIdentity()
            {
            if (this._wi == null)
                {
                bool result = LogonUser(this._username, this._domain, this._password, LogonSessionType.Interactive, LogonProvider.Default, out _originalToken);
                if (!result)
                    {
                    System.Diagnostics.Trace.WriteLine("LogonUser failed.");
                    throw new Win32Exception();
                    }
       
                result = DuplicateToken(_originalToken.DangerousGetHandle(), SecurityImpersonationLevel.SecurityImpersonation, out _duplicatedToken);
                if (!result)
                    {
                    System.Diagnostics.Trace.WriteLine("DuplicateToken failed.");
                    throw new Win32Exception();
                    }

                this._wi = new WindowsIdentity(_duplicatedToken.DangerousGetHandle());
                }

            return this._wi;
            }

        /// <summary>
        /// Disposes the resources associated with this object
        /// </summary>
        public void Dispose()
            {
            if (this._wi != null)
                {
                this._wi.Dispose();
                this._wi = null;
                }

            if (this._duplicatedToken != null)
                {
                this._duplicatedToken.Dispose();
                this._duplicatedToken = null;
                }

            if (this._originalToken != null)
                {
                this._originalToken.Dispose();
                this._originalToken = null;
                }
            }
        }
    }
