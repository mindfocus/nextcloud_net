using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ContactsNs.ContactsMenu
{
    /**
     * @since 12.0
     */
    public interface IProvider
    {

        /**
         * @since 12.0
         * @param IEntry entry
         * @return void
         */
        void process(IEntry entry);
    }

}
