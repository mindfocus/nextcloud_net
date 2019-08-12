using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Resource
{
    /**
     * Interface IManager
     *
     * @package OCP\Calendar\Resource
     * @since 14.0.0
     */
    public interface IManager
    {

        /**
         * Registers a resource backend
         *
         * @param string backendClass
         * @return void
         * @since 14.0.0
         */
        void registerBackend(string backendClass);

        /**
         * Unregisters a resource backend
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
