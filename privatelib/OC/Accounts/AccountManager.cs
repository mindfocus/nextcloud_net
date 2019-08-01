using System;
using System.Linq;
using model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OCP;
using OCP.Accounts;
using OCP.BackgroundJob;

namespace OC.Accounts
{
    /*
     * Class AccountManager
     *
     * Manage system accounts table
     *
     * @group DB
     * @package OC\Accounts
     */
    public class AccountManager : IAccountManager
    {

        /** @var EventDispatcherInterface */
        private OCP.Sym.EventDispatcherInterface eventDispatcher;

        /** @var IJobList */
        private IJobList jobList;


        // <summary>
        // Initializes a new instance of the <see cref="T:OC.Accounts.AccountManager"/> class.
        // </summary>
        // <param name="connection">Connection.</param>
        // <param name="eventDispatcher">Event dispatcher.</param>
        // <param name="jobList">Job list.</param>
        public AccountManager(OCP.Sym.EventDispatcherInterface eventDispatcher,
                                    IJobList jobList)
        {
            this.eventDispatcher = eventDispatcher;
            this.jobList = jobList;
        }

        private Account parseAccountData(IUser user, JObject accountData)
        {
            var account = new Account(user);

            foreach (var property in accountData)
            {
                account.setProperty(property.Key,
                    property.Value["value"].ToObject<string>() ?? "",
                    property.Value["scope"].ToObject<string>() ?? AccountVisibility.VISIBILITY_PRIVATE.Value,
                    property.Value["verified"].ToObject<string>() ?? AccountVerified.NOT_VERIFIED.Value);
            }
            return account;
        }

        public IAccount getAccount(OCP.IUser user)
        {
            return this.parseAccountData(user, this.getUser(user));
        }
        // <summary>
        // update user record
        // </summary>
        // <param name="user">user</param>
        // <param name="data"></param>
        public void updateUser(OCP.IUser user, JObject data)
        {
            var userData = this.getUser(user);
            var updated = true;
            if (userData == null)
            {
                this.insertNewUser(user, data);
            }
            else if (userData != data)
            {
                data = this.checkEmailVerification(userData, data, user);
                data = this.updateVerifyStatus(userData, data);
                this.updateExistingUser(user, data);
            }
            else
            {
                // nothing needs to be done if new and old data set are the same
                updated = false;
            }

            if (updated)
            {
                //this.eventDispatcher.dispatch(
                //    'OC\AccountManager::userUpdated',
                //    new GenericEvent(user, data)
                //);
            }
        }

        /**
         * delete user from accounts table
         *
         * @param IUser user
         */
        public void deleteUser(IUser user)
        {
            var uid = user.getUID();
            AccountTable account = null;
            using (var context = new NCContext())
            {
                account = context.Accounts.Find(uid);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    context.SaveChanges();
                }
            }
        }

        /**
         * get stored data from a given user
         *
         * @param IUser user
         * @return array
         */
        public JObject getUser(IUser user)
        {
            var uid = user.getUID();
            AccountTable account = null;
            using (var context = new NCContext())
            {
                account = context.Accounts.Find(uid);
            }
            if (account == null)
            {
                var userData = this.buildDefaultUserRecord(user);
                this.insertNewUser(user, userData);
                return userData;
            }

            //userDataArray = json_decode(result[0]['data'], true);

            //userDataArray = this.addMissingDefaultValues(userDataArray);

            return JObject.Parse(account.data);
        }

        /**
         * check if we need to ask the server for email verification, if yes we create a cronjob
         *
         * @param oldData
         * @param newData
         * @param IUser user
         * @return array
         */
        protected JObject checkEmailVerification(JObject oldData, JObject newData, IUser user)
        {
            if (oldData.Root.Value<string>(AccountCommonProperty.EMAIL.Value) != newData.Root.Value<string>(AccountCommonProperty.EMAIL.Value))
            {
                // @focus
                //this.jobList.add(VerifyUserData::class,
                //[
                //    'verificationCode' => '',
                //    'data' => newData[self::PROPERTY_EMAIL] ['value'],
                //    'type' => self::PROPERTY_EMAIL,
                //    'uid' => user.getUID(),
                //    'try' => 0,
                //    'lastRun' => time()
                //]
                //);
                newData[AccountCommonProperty.EMAIL.Value]["verified"] = AccountVerified.VERIFICATION_IN_PROGRESS.Value;
            }
            return newData;
        }

        /*
         * make sure that all expected data are set
         *
         * @param array userData
         * @return array
         */
        protected JObject addMissingDefaultValues(JObject userData)
        {
            foreach (var child in userData)
            {
                var value = child.Value;
                var dic = ((JObject)value).Properties().ToDictionary(p => p.Name, p => p.Value);
                if (dic.ContainsKey(AccountVerified.Name))
                {
                    dic[AccountVerified.Name] = AccountVerified.NOT_VERIFIED.Value.ToString();
                }
                var dicstr = JsonConvert.SerializeObject(dic, Formatting.Indented);
                userData[value.Path] = JObject.Parse(dicstr);
            }
            return userData;
        }

        /**
         * reset verification status if personal data changed
         *
         * @param array oldData
         * @param array newData
         * @return array
         */
        protected JObject updateVerifyStatus(JObject oldData, JObject newData)
        {

            // which account was already verified successfully?
            var twitterVerified = oldData[AccountCommonProperty.TWITTER.Value]["verified"].ToObject<bool>() &&
                oldData[AccountCommonProperty.TWITTER.Value]["verified"].ToObject<bool>();
            var websiteVerified = oldData[AccountCommonProperty.WEBSITE.Value]["verified"].ToObject<bool>() &&
                oldData[AccountCommonProperty.WEBSITE.Value]["verified"].ToObject<bool>();
            var emailVerified = oldData[AccountCommonProperty.EMAIL.Value]["verified"].ToObject<bool>() &&
                oldData[AccountCommonProperty.EMAIL.Value]["verified"].ToObject<bool>();

            //// keep old verification status if we don't have a new one
            //if (!isset(newData[self::PROPERTY_TWITTER]['verified']))
            //{
            //    // keep old verification status if value didn't changed and an old value exists
            //    keepOldStatus = newData[self::PROPERTY_TWITTER]['value'] === oldData[self::PROPERTY_TWITTER]['value'] && isset(oldData[self::PROPERTY_TWITTER]['verified']);
            //    newData[self::PROPERTY_TWITTER]['verified'] = keepOldStatus ? oldData[self::PROPERTY_TWITTER]['verified'] : self::NOT_VERIFIED;
            //}

            //if (!isset(newData[self::PROPERTY_WEBSITE]['verified']))
            //{
            //    // keep old verification status if value didn't changed and an old value exists
            //    keepOldStatus = newData[self::PROPERTY_WEBSITE]['value'] === oldData[self::PROPERTY_WEBSITE]['value'] && isset(oldData[self::PROPERTY_WEBSITE]['verified']);
            //    newData[self::PROPERTY_WEBSITE]['verified'] = keepOldStatus ? oldData[self::PROPERTY_WEBSITE]['verified'] : self::NOT_VERIFIED;
            //}

            //if (!isset(newData[self::PROPERTY_EMAIL]['verified']))
            //{
            //    // keep old verification status if value didn't changed and an old value exists
            //    keepOldStatus = newData[self::PROPERTY_EMAIL]['value'] === oldData[self::PROPERTY_EMAIL]['value'] && isset(oldData[self::PROPERTY_EMAIL]['verified']);
            //    newData[self::PROPERTY_EMAIL]['verified'] = keepOldStatus ? oldData[self::PROPERTY_EMAIL]['verified'] : self::VERIFICATION_IN_PROGRESS;
            //}

            //// reset verification status if a value from a previously verified data was changed
            //if (twitterVerified &&
            //        oldData[self::PROPERTY_TWITTER]['value'] !== newData[self::PROPERTY_TWITTER]['value']
            //    )
            //{
            //    newData[self::PROPERTY_TWITTER]['verified'] = self::NOT_VERIFIED;
            //}

            //if (websiteVerified &&
            //        oldData[self::PROPERTY_WEBSITE]['value'] !== newData[self::PROPERTY_WEBSITE]['value']
            //    )
            //{
            //    newData[self::PROPERTY_WEBSITE]['verified'] = self::NOT_VERIFIED;
            //}

            //if (emailVerified &&
            //        oldData[self::PROPERTY_EMAIL]['value'] !== newData[self::PROPERTY_EMAIL]['value']
            //    )
            //{
            //    newData[self::PROPERTY_EMAIL]['verified'] = self::NOT_VERIFIED;
            //}

            return newData;

        }

        /**
         * add new user to accounts table
         *
         * @param IUser user
         * @param array data
         */
        protected void insertNewUser(IUser user, object data)
        {
            //var uid = user.getUID();
            //jsonEncodedData = json_encode(data);
            //query = this.connection.getQueryBuilder();
            //query.insert(this.table)
            //    .values(

            //        [
            //            'uid' => query.createNamedParameter(uid),
            //            'data' => query.createNamedParameter(jsonEncodedData),

            //        ]
            //        )
            //        .execute();
        }

        /**
         * update existing user in accounts table
         *
         * @param IUser user
         * @param array data
         */
        protected void updateExistingUser(IUser user, JObject data)
        {
            var uid = user.getUID();
            AccountTable account = null;
            using (var context = new NCContext())
            {
                account = context.Accounts.Find(uid);
                if (account != null)
                {
                    account.data = data.ToString();
                    context.SaveChanges();
                }
            }
            //jsonEncodedData = json_encode(data);
            //query = this.connection.getQueryBuilder();
            //query.update(this.table)
            //    .set('data', query.createNamedParameter(jsonEncodedData))
            //    .where(query.expr().eq('uid', query.createNamedParameter(uid)))
            //    .execute();
        }

        /**
         * build default user record in case not data set exists yet
         *
         * @param IUser user
         * @return array
         */
        protected JObject buildDefaultUserRecord(IUser user)
        {
            return
    new JObject(
        new JProperty(AccountCommonProperty.DISPLAYNAME.Value,
            new JObject(
                new JProperty("value", user.getDisplayName()),
                new JProperty("scope", AccountVisibility.VISIBILITY_CONTACTS_ONLY),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           )),
                new JProperty(AccountCommonProperty.ADDRESS.Value,
            new JObject(
                new JProperty("value", ""),
                new JProperty("scope", AccountVisibility.VISIBILITY_PRIVATE),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           )),
                                new JProperty(AccountCommonProperty.WEBSITE.Value,
            new JObject(
                new JProperty("value", ""),
                new JProperty("scope", AccountVisibility.VISIBILITY_PRIVATE),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           )), new JProperty(AccountCommonProperty.EMAIL.Value,
            new JObject(
                new JProperty("value", user.getEMailAddress()),
                new JProperty("scope", AccountVisibility.VISIBILITY_PRIVATE),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           )),
                                new JProperty(AccountCommonProperty.AVATAR.Value,
            new JObject(
                new JProperty("scope", AccountVisibility.VISIBILITY_CONTACTS_ONLY)
           )),
                                new JProperty(AccountCommonProperty.PHONE.Value,
            new JObject(
                new JProperty("value", user.getEMailAddress()),
                new JProperty("scope", AccountVisibility.VISIBILITY_PRIVATE),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           )),
                                new JProperty(AccountCommonProperty.TWITTER.Value,
            new JObject(
                new JProperty("value", ""),
                new JProperty("scope", AccountVisibility.VISIBILITY_PRIVATE),
                new JProperty("verified", AccountVerified.NOT_VERIFIED)
           ))
        );
        }


    }
}

