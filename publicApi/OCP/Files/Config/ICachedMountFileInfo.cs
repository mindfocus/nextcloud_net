using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.Config
{
    /**
     * Holds information about a mount for a user
     *
     * @since 13.0.0
     */
    public interface ICachedMountFileInfo : ICachedMountInfo
    {
    /**
	 * Return the path for the file within the cached mount
	 *
	 * @return string
	 * @since 13.0.0
	 */
    string  getInternalPath();

    /**
	 * @return string
	 * @since 13.0.0
	 */
    string getPath();
}

}
