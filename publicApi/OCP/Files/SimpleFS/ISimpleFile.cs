using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files.SimpleFS
{
    /**
     * Interface ISimpleFile
     *
     * @package OCP\Files\SimpleFS
     * @since 11.0.0
     */
    public interface ISimpleFile
    {

        /**
         * Get the name
         *
         * @return string
         * @since 11.0.0
         */
        string getName();

        /**
         * Get the size in bytes
         *
         * @return int
         * @since 11.0.0
         */
        int getSize();

        /**
         * Get the ETag
         *
         * @return string
         * @since 11.0.0
         */
        string getETag();

        /**
         * Get the last modification time
         *
         * @return int
         * @since 11.0.0
         */
        long getMTime();

        /**
         * Get the content
         *
         * @throws NotPermittedException
         * @throws NotFoundException
         * @return string
         * @since 11.0.0
         */
        string getContent();

        /**
         * Overwrite the file
         *
         * @param string|resource data
         * @throws NotPermittedException
         * @since 11.0.0
         */
        void putContent(string data);

        /**
         * Delete the file
         *
         * @throws NotPermittedException
         * @since 11.0.0
         */
        void delete();

        /**
         * Get the MimeType
         *
         * @return string
         * @since 11.0.0
         */
        string getMimeType();

        /**
         * Open the file as stream for reading, resulting resource can be operated as stream like the result from php's own fopen
         *
         * @return resource
         * @throws \OCP\Files\NotPermittedException
         * @since 14.0.0
         */
        int read();

        /**
         * Open the file as stream for writing, resulting resource can be operated as stream like the result from php's own fopen
         *
         * @return resource
         * @throws \OCP\Files\NotPermittedException
         * @since 14.0.0
         */
        int write();
    }

}
