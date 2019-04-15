using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.Accounts
{
    interface IAccountManager
    {
        /**
 * Get the account data for a given user
 *
 * @since 15.0.0
 *
 * @param IUser $user
 * @return IAccount
 */
        IAccount getAccount(IUser user);

        ///** nobody can see my account details */
        //const string VISIBILITY_PRIVATE = 'private';
        ///** only contacts, especially trusted servers can see my contact details */
        //const VISIBILITY_CONTACTS_ONLY = 'contacts';
        ///** every body ca see my contact detail, will be published to the lookup server */
        //const VISIBILITY_PUBLIC = 'public';

        //const PROPERTY_AVATAR = 'avatar';
        //const PROPERTY_DISPLAYNAME = 'displayname';
        //const PROPERTY_PHONE = 'phone';
        //const PROPERTY_EMAIL = 'email';
        //const PROPERTY_WEBSITE = 'website';
        //const PROPERTY_ADDRESS = 'address';
        //const PROPERTY_TWITTER = 'twitter';

        //const NOT_VERIFIED = '0';
        //const VERIFICATION_IN_PROGRESS = '1';
        //const VERIFIED = '2';
    }
}
