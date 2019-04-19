using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar.Room
{
    /**
     * Interface IBackend
     *
     * @package OCP\Calendar\Room
     * @since 14.0.0
     */
    public interface IBackend
    {

        /**
         * get a list of all rooms in this backend
         *
         * @throws BackendTemporarilyUnavailableException
         * @return IRoom[]
         * @since 14.0.0
         */
        IList<IRoom> getAllRooms();

	/**
	 * get a list of all room identifiers in this backend
	 *
	 * @throws BackendTemporarilyUnavailableException
	 * @return string[]
	 * @since 14.0.0
	 */
	IList<string> listAllRooms();

	/**
	 * get a room by it's id
	 *
	 * @param string $id
	 * @throws BackendTemporarilyUnavailableException
	 * @return IRoom|null
	 * @since 14.0.0
	 */
	IRoom? getRoom(string id);

        /**
         * Get unique identifier of the backend
         *
         * @return string
         * @since 14.0.0
         */
        string getBackendIdentifier();
}

}
