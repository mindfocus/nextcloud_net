using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.SimpleFS
{
    /**
     * Interface ISimpleFolder
     *
     * @package OCP\Files\SimpleFS
     * @since 11.0.0
     */
    public interface ISimpleFolder
    {
        /**
         * Get all the files in a folder
         *
         * @return ISimpleFile[]
         * @since 11.0.0
         */
        IList<ISimpleFile> getDirectoryListing();

        /**
         * Check if a file with name exists
         *
         * @param string name
         * @return bool
         * @since 11.0.0
         */
        bool fileExists(string name);

        /**
         * Get the file named name from the folder
         *
         * @param string name
         * @return ISimpleFile
         * @throws NotFoundException
         * @since 11.0.0
         */
        ISimpleFile getFile(string name);

        /**
         * Creates a new file with name in the folder
         *
         * @param string name
         * @return ISimpleFile
         * @throws NotPermittedException
         * @since 11.0.0
         */
        ISimpleFile newFile(string name);

        /**
         * Remove the folder and all the files in it
         *
         * @throws NotPermittedException
         * @since 11.0.0
         */
        void delete();

        /**
         * Get the folder name
         *
         * @return string
         * @since 11.0.0
         */
        string getName();
    }

}
