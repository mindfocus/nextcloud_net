using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Capabilities
{
    /**
     * Minimal interface that has to be implemented for a class to be considered
     * a capability.
     *
     * In an application use:
     *   $this->getContainer()->registerCapability('OCA\MY_APP\Capabilities');
     * To register capabilities.
     *
     * The class 'OCA\MY_APP\Capabilities' must then implement ICapability
     *
     * @since 8.2.0
     */
    interface ICapability
    {

        /**
         * Function an app uses to return the capabilities
         *
         * @return array Array containing the apps capabilities
         * @since 8.2.0
         */
        IList<string> getCapabilities();
    }

}
