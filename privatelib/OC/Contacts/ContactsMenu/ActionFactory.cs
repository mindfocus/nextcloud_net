using OC.Contacts.ContactsMenu.Actions;
using OCP.ContactsNs.ContactsMenu;

namespace OC.Contacts.ContactsMenu
{
    public class ActionFactory : IActionFactory {

    /**
     * @param string icon
     * @param string name
     * @param string href
     * @return ILinkAction
     */
    public ILinkAction newLinkAction(string icon, string name, string href) {
        var action = new LinkAction();
            action.setName(name);
        action.setIcon(icon);
        action.setHref(href);
        return action;
    }

    /**
     * @param string icon
     * @param string name
     * @param string email
     * @return ILinkAction
     */
    public ILinkAction newEMailAction(string icon, string name, string email) {
        return this.newLinkAction(icon, name, "mailto:" + (email));
        // TODO add urlencode function
//        return this.newLinkAction(icon, name, "mailto:" + urlencode(email));
    }

    }
}