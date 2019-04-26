using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ContactsNs.ContactsMenu
{
    /**
     * @since 12.0
     */
    interface IActionFactory
    {

        /**
         * Construct and return a new link action for the contacts menu
         *
         * @since 12.0
         *
         * @param string icon full path to the action's icon
         * @param string name localized name of the action
         * @param string href target URL
         * @return ILinkAction
         */
        ILinkAction newLinkAction(string icon, string name, string href);

        /**
         * Construct and return a new email action for the contacts menu
         *
         * @since 12.0
         *
         * @param string icon full path to the action's icon
         * @param string name localized name of the action
         * @param string email target e-mail address
         * @return ILinkAction
         */
        ILinkAction newEMailAction(string icon, string name, string email);
    }

}
