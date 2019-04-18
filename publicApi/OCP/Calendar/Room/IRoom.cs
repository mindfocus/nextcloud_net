using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Room
{
    /**
     * Interface IRoom
     *
     * @package OCP\Calendar\Room
     * @since 14.0.0
     */
    interface IRoom
    {

        /**
         * get the room id
         *
         * This id has to be unique within the backend
         *
         * @return string
         * @since 14.0.0
         */
        string getId();

	/**
	 * get the display name for a room
	 *
	 * @return string
	 * @since 14.0.0
	 */
	string getDisplayName();

	/**
	 * Get a list of groupIds that are allowed to access this room
	 *
	 * If an empty array is returned, no group restrictions are
	 * applied.
	 *
	 * @return string[]
	 * @since 14.0.0
	 */
	IList<string> getGroupRestrictions();

	/**
	 * get email-address for room
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
