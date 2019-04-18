using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.Contacts.ContactsMenu
{
    /**
     * @since 12.0
     */
    interface IProvider
    {

        /**
         * @since 12.0
         * @param IEntry $entry
         * @return void
         */
        void process(IEntry entry);
    }

}
