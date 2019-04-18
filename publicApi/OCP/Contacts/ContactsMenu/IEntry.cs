using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OCP.Contacts.ContactsMenu
{
    interface IEntry : ISerializable
    {
        /**
 * @since 12.0
 * @return string
 */
        string getFullName();

        /**
         * @since 12.0
         * @return string[]
         */
        IList<string> getEMailAddresses();

        /**
         * @since 12.0
         * @return string|null image URI
         */
        string? getAvatar();

        /**
         * @since 12.0
         * @param IAction $action an action to show in the contacts menu
         */
        void addAction(IAction action);

        /**
         * Get an arbitrary property from the contact
         *
         * @since 12.0
         * @param string $key
         * @return mixed the value of the property or null
         */
        object getProperty(string key);
    }
}
