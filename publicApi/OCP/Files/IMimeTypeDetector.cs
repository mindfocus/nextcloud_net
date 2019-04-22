using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{

    /**
     * Interface IMimeTypeDetector
     * @package OCP\Files
     * @since 8.2.0
     *
     * Interface to handle mimetypes (detection and icon retrieval)
     **/
    public interface IMimeTypeDetector
    {

        /**
         * detect mimetype only based on filename, content of file is not used
         * @param string path
         * @return string
         * @since 8.2.0
         **/
        string detectPath(string path);

        /**
         * detect mimetype based on both filename and content
         *
         * @param string path
         * @return string
         * @since 8.2.0
         */
        string detect(string path);

        /**
         * Get a secure mimetype that won't expose potential XSS.
         *
         * @param string mimeType
         * @return string
         * @since 8.2.0
         */
        string getSecureMimeType(string mimeType);

        /**
         * detect mimetype based on the content of a string
         *
         * @param string data
         * @return string
         * @since 8.2.0
         */
        string detectString(string data);

        /**
         * Get path to the icon of a file type
         * @param string mimeType the MIME type
         * @return string the url
         * @since 8.2.0
         */
        string mimeTypeIcon(string mimeType);
    }

}
