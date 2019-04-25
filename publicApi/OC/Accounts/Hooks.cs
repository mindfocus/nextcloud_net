using System;
using System.Collections.Generic;
using System.Text;
using OCP;

namespace OC.Accounts
{
    class Hooks {

        /** @var  AccountManager */
        private AccountManager accountManager = null;

        /** @var ILogger */
        private OCP.ILogger logger;

        /**
         * Hooks constructor.
         *
         * @param ILogger logger
         */
        public Hooks(OCP.ILogger logger) {
            this.logger = logger;
        }
        /**
 * return instance of accountManager
 *
 * @return AccountManager
 */
        protected AccountManager getAccountManager() {
            if (this.accountManager == null)
            {
            }
            //if (is_null(this.accountManager)) {
            //    this.accountManager = new AccountManager(
            //        \OC::server.getDatabaseConnection(),
            //        \OC::server.getEventDispatcher(),
            //        \OC::server.getJobList()
            //        );
            //}
            return this.accountManager;
        }
        /**
         * update accounts table if email address or display name was changed from outside
         *
         * @param array params
         */
        public void changeUserHook(IDictionary<string,object> paramList) {

            accountManager = this.getAccountManager();

                /** @var IUser user */
                var user = paramList.ContainsKey("user") ? (IUser) paramList["user"] : null;
                var feature = paramList.ContainsKey("feature") ? paramList["feature"] : null;
                var newValue = paramList.ContainsKey("value");
                if (user == null || feature == null || newValue == null)
                {
                    this.logger.warning("Missing expected parameters in change user hook");
                    return;
                }
            var accountData = accountManager.getUser(user);

            switch (feature) {
                case "eMailAddress":
                if (accountData[OCP.Accounts.AccountCommonProperty.EMAIL.Value]["value"] != newValue) {
                    accountData[OCP.Accounts.AccountCommonProperty.EMAIL.Value]["value"] = newValue;
                        accountManager.updateUser(user, accountData);
                }
                break;
                case "displayName":
                if (accountData[OCP.Accounts.AccountCommonProperty.DISPLAYNAME.Value]["value"] != newValue) {
                    accountData[OCP.Accounts.AccountCommonProperty.DISPLAYNAME.Value]["value"] = newValue;
                        accountManager.updateUser(user, accountData);
                }
                break;
            }

        }



    }

}
