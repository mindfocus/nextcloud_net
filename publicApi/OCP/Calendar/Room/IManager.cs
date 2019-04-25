using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Room
{
    /**
     * Interface IManager
     *
     * @package OCP\Calendar\Room
     * @since 14.0.0
     */
    interface IManager
    {

        /**
         * Registers a room backend
         *
         * @param string backendClass
         * @return void
         * @since 14.0.0
         */
        void registerBackend(string backendClass);

        /**
         * Unregisters a room backend
         *
         * @param string backendClass
         * @return void
         * @since 14.0.0
         */
        void unregisterBackend(string backendClass);

        /**
         * @return IBackend[]
         * @since 14.0.0
         */
        IList<IBackend> getBackends();

	/**
	 * @param string backendId
	 * @return IBackend|null
	 * @since 14.0.0
	 */
	IBackend getBackend(string backendId);

        /**
         * removes all registered backend instances
         * @return void
         * @since 14.0.0
         */
        void clear();
    }

}
