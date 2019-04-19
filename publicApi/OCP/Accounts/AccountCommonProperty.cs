using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Accounts
{

    public class AccountCommonProperty
    {
        public string Value { get; internal set; }
        private AccountCommonProperty(string value)
        {
            this.Value = value;
        }
        public static AccountCommonProperty AVATAR { get { return new AccountCommonProperty("avatar"); } }
        public static AccountCommonProperty DISPLAYNAME { get { return new AccountCommonProperty("displayname"); } }
        public static AccountCommonProperty PHONE { get { return new AccountCommonProperty("phone"); } }
        public static AccountCommonProperty EMAIL { get { return new AccountCommonProperty("email"); } }
        public static AccountCommonProperty WEBSITE { get { return new AccountCommonProperty("website"); } }
        public static AccountCommonProperty ADDRESS { get { return new AccountCommonProperty("address"); } }
        public static AccountCommonProperty TWITTER { get { return new AccountCommonProperty("twitter"); } }

    }
}
