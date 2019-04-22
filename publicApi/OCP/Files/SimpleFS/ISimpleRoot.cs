using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.SimpleFS
{
    /**
     * Interface ISimpleRoot
     *
     * @package OCP\Files\SimpleFS
     * @since 11.0.0
     */
    public interface ISimpleRoot
    {
        /**
         * Get the folder with name $name
         *
         * @param string $name
         * @return ISimpleFolder
         * @throws NotFoundException
         * @throws \RuntimeException
         * @since 11.0.0
         */
        ISimpleFolder getFolder(string name);

	/**
	 * Get all the Folders
	 *
	 * @return ISimpleFolder[]
	 * @throws NotFoundException
	 * @throws \RuntimeException
	 * @since 11.0.0
	 */
	IList<ISimpleFolder> getDirectoryListing();

        /**
         * Create a new folder named $name
         *
         * @param string $name
         * @return ISimpleFolder
         * @throws NotPermittedException
         * @throws \RuntimeException
         * @since 11.0.0
         */
        ISimpleFolder newFolder(string name);
}

}
