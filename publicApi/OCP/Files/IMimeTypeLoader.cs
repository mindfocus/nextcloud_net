using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
     * Interface IMimeTypeLoader
     * @package OCP\Files
     * @since 8.2.0
     *
     * Interface to load mimetypes
     **/
    public interface IMimeTypeLoader
    {

        /**
         * Get a mimetype from its ID
         *
         * @param int $id
         * @return string|null
         * @since 8.2.0
         */
        string? getMimetypeById(int id);

        /**
         * Get a mimetype ID, adding the mimetype to the DB if it does not exist
         *
         * @param string $mimetype
         * @return int
         * @since 8.2.0
         */
        int getId(string mimetype);

        /**
         * Test if a mimetype exists in the database
         *
         * @param string $mimetype
         * @return bool
         * @since 8.2.0
         */
        bool exists(string mimetype);

        /**
         * Clear all loaded mimetypes, allow for re-loading
         *
         * @since 8.2.0
         */
        void reset();
    }

}
