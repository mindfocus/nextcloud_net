using OC.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
     * Interface IRootFolder
     *
     * @package OCP\Files
     * @since 8.0.0
     */
    public interface IRootFolder : Folder, Emitter
    {

	/**
	 * Returns a view to user's files folder
	 *
	 * @param string userId user ID
	 * @return \OCP\Files\Folder
	 * @since 8.2.0
	 */
	Folder getUserFolder(string userId);
}
}
