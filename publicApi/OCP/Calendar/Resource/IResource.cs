using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Resource
{
    /**
     * Interface IResource
     *
     * @package OCP\Calendar\Resource
     * @since 14.0.0
     */
    interface IResource
    {

        /**
         * get the resource id
         *
         * This id has to be unique within the backend
         *
         * @return string
         * @since 14.0.0
         */
        string getId();

	/**
	 * get the display name for a resource
	 *
	 * @return string
	 * @since 14.0.0
	 */
	string getDisplayName();

	/**
	 * Get a list of groupIds that are allowed to access this resource
	 *
	 * If an empty array is returned, no group restrictions are
	 * applied.
	 *
	 * @return string[]
	 * @since 14.0.0
	 */
	IList<string> getGroupRestrictions();

	/**
	 * get email-address for resource
	 *
	 * The email address has to be globally unique
	 *
	 * @return string
	 * @since 14.0.0
	 */
	string getEMail();

	/**
	 * Get corresponding backend object
	 *
	 * @return IBackend
	 * @since 14.0.0
	 */
	IBackend getBackend();
}

}
