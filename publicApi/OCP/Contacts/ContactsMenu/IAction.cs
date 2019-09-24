using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OCP.ContactsNs.ContactsMenu
{
    /**
     * Apps should use the IActionFactory to create new action objects
     *
     * @since 12.0
     */
    public interface IAction : ISerializable
    {

    /**
	 * @param string icon absolute URI to an icon
	 * @since 12.0
	 */
    void setIcon(string icon);

    /**
	 * @return string localized action name, e.g. 'Call'
	 * @since 12.0
	 */
    string getName();

    /**
	 * @param string name localized action name, e.g. 'Call'
	 * @since 12.0
	 */
    void setName(string name);

    /**
	 * @param int priority priorize actions, high order ones are shown on top
	 * @since 12.0
	 */
    void setPriority(int priority);

    /**
	 * @return int priority to priorize actions, high order ones are shown on top
	 * @since 12.0
	 */
    int getPriority();
}

}
