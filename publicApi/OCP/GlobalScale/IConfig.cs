using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.GlobalScale
{

    /**
     * Interface IConfig
     *
     * Configuration of the global scale architecture
     *
     * @package OCP\GlobalScale
     * @since 12.0.1
     */
    public interface IConfig
    {

        /**
         * check if global scale is enabled
         *
         * @since 12.0.1
         * @return bool
         */
        bool isGlobalScaleEnabled();

        /**
         * check if federation should only be used internally in a global scale setup
         *
         * @since 12.0.1
         * @return bool
         */
        bool onlyInternalFederation();

    }
}
