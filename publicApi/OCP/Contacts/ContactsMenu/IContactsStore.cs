using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Contacts.ContactsMenu
{

    /**
     * @since 13.0.0
     */
    interface IContactsStore
    {


        /**
         * @param IUser $user
         * @param $filter
         * @return IEntry[]
         * @since 13.0.0
         */
        IList<IEntry> getContacts(IUser user, string filter);

        /**
         * @brief finds a contact by specifying the property to search on ($shareType) and the value ($shareWith)
         * @param IUser $user
         * @param integer $shareType
         * @param string $shareWith
         * @return IEntry|null
         * @since 13.0.0
         */
        IEntry? findOne(IUser user, int shareType, string shareWith);

    }

}
