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
        public static AccountCommonProperty AVATAR => new AccountCommonProperty("avatar");
        public static AccountCommonProperty DISPLAYNAME => new AccountCommonProperty("displayname");
        public static AccountCommonProperty PHONE => new AccountCommonProperty("phone");
        public static AccountCommonProperty EMAIL => new AccountCommonProperty("email");
        public static AccountCommonProperty WEBSITE => new AccountCommonProperty("website");
        public static AccountCommonProperty ADDRESS => new AccountCommonProperty("address");
        public static AccountCommonProperty TWITTER => new AccountCommonProperty("twitter");
    }
}
