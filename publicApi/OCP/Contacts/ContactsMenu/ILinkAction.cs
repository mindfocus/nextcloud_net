using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ContactsNs.ContactsMenu
{
    /**
     * @since 12.0
     */
    public interface ILinkAction : IAction
    {

    /**
	 * @since 12.0
	 * @param string href the target URL of the action
	 */
    void setHref(string href);

    /**
	 * @since 12.0
	 * @return string
	 */
    string getHref();
}

}
