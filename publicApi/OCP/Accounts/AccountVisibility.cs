using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Accounts
{
    public class AccountVisibility
    {
        public string Value { get; internal set; }
        private AccountVisibility(string value)
        {
            this.Value = value;
        }
        public static AccountVisibility VISIBILITY_PRIVATE { get { return new AccountVisibility("private"); } }
        public static AccountVisibility VISIBILITY_CONTACTS_ONLY { get { return new AccountVisibility("contacts"); } }
        public static AccountVisibility VISIBILITY_PUBLIC { get { return new AccountVisibility("public"); } }
    }
}
