using System;
using System.Collections.Generic;
using System.Text;
using OC.Files;
namespace OCP.Preview
{
    /**
     * Interface IProvider
     *
     * @package OCP\Preview
     * @since 8.1.0
     */
    public interface IProvider
    {
        /**
         * @return string Regex with the mimetypes that are supported by this provider
         * @since 8.1.0
         */
        string getMimeType();

        /**
         * Check if a preview can be generated for path
         *
         * @param \OCP\Files\FileInfo file
         * @return bool
         * @since 8.1.0
         */
        bool isAvailable(Files.FileInfo file);

        /**
         * get thumbnail for file at path path
         *
         * @param string path Path of file
         * @param int maxX The maximum X size of the thumbnail. It can be smaller depending on the shape of the image
         * @param int maxY The maximum Y size of the thumbnail. It can be smaller depending on the shape of the image
         * @param bool scalingup Disable/Enable upscaling of previews
         * @param \OC\Files\View fileview fileview object of user folder
         * @return bool|\OCP\IImage false if no preview was generated
         * @since 8.1.0
         */
        IImage? getThumbnail(string path, int maxX, int maxY, bool scalingup, View fileview);
    }

}
