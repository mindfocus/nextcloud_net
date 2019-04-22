using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Resource
{
    /**
     * Interface IBackend
     *
     * @package OCP\Calendar\Resource
     * @since 14.0.0
     */
    public interface IBackend
    {

        /**
         * get a list of all resources in this backend
         *
         * @throws BackendTemporarilyUnavailableException
         * @return IResource[]
         * @since 14.0.0
         */
        IList<IResource> getAllResources();

	/**
	 * get a list of all resource identifiers in this backend
	 *
	 * @throws BackendTemporarilyUnavailableException
	 * @return string[]
	 * @since 14.0.0
	 */
	IList<string> listAllResources();

	/**
	 * get a resource by it's id
	 *
	 * @param string id
	 * @throws BackendTemporarilyUnavailableException
	 * @return IResource|null
	 * @since 14.0.0
	 */
	IResource? getResource(string id);

        /**
         * Get unique identifier of the backend
         *
         * @return string
         * @since 14.0.0
         */
        string getBackendIdentifier();
}
}
