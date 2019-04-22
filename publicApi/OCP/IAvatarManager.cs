using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * This class provides avatar functionality
     * @since 6.0.0
     */

    public interface IAvatarManager
    {

        /**
         * return a user specific instance of \OCP\IAvatar
         * @see IAvatar
         * @param string $user the ownCloud user id
         * @return IAvatar
         * @throws \Exception In case the username is potentially dangerous
         * @throws \OCP\Files\NotFoundException In case there is no user folder yet
         * @since 6.0.0
         */
        IAvatar getAvatar(string user) ;

	/**
	 * Returns a guest user avatar instance.
	 *
	 * @param string $name The guest name, e.g. "Albert".
	 * @return IAvatar
	 * @since 16.0.0
	 */
	IAvatar getGuestAvatar(string name);

}

}
