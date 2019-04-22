using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Accounts
{
    /// <summary>
    /// const NOT_VERIFIED = '0'
    /// const VERIFICATION_IN_PROGRESS = '1'
    /// const VERIFIED = '2'
    /// </summary>
    public class AccountVerified
    {

        public string Value { get; internal set; }
        public static string Name => "verified";
        private AccountVerified(string value)
        {
            this.Value = value;
        }
        public static AccountVerified NOT_VERIFIED { get { return new AccountVerified("0"); } }
        public static AccountVerified VERIFICATION_IN_PROGRESS { get { return new AccountVerified("1"); } }
        public static AccountVerified VERIFIED { get { return new AccountVerified("2"); } }
    }
}
