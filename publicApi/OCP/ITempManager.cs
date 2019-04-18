using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface ITempManager
     *
     * @package OCP
     * @since 8.0.0
     */
    public interface ITempManager
    {
        /**
         * Create a temporary file and return the path
         *
         * @param string $postFix
         * @return string
         * @since 8.0.0
         */
        string getTemporaryFile(string postFix = "");

        /**
         * Create a temporary folder and return the path
         *
         * @param string $postFix
         * @return string
         * @since 8.0.0
         */
        string getTemporaryFolder(string postFix = "");

        /**
         * Remove the temporary files and folders generated during this request
         * @since 8.0.0
         */
        void clean();

        /**
         * Remove old temporary files and folders that were failed to be cleaned
         * @since 8.0.0
         */
        void cleanOld();

        /**
         * Get the temporary base directory
         *
         * @return string
         * @since 8.2.0
         */
        string getTempBaseDir();
    }

}
