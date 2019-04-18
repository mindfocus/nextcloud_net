﻿using OCP.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * This class provides functions to render and show thumbnails and previews of files
     * @since 6.0.0
     */
    interface IPreview
    {

        /**
         * @since 9.2.0
         */
 //       const EVENT = self::class . ':' . 'PreviewRequested';

	//const MODE_FILL = 'fill';
 //       const MODE_COVER = 'cover';

        /**
         * In order to improve lazy loading a closure can be registered which will be
         * called in case preview providers are actually requested
         *
         * $callable has to return an instance of \OCP\Preview\IProvider
         *
         * @param string $mimeTypeRegex Regex with the mime types that are supported by this provider
         * @param \Closure $callable
         * @return void
         * @since 8.1.0
         */
        void registerProvider(string mimeTypeRegex, Action callable);

        /**
         * Get all providers
         * @return array
         * @since 8.1.0
         */
        IList<string> getProviders();

        /**
         * Does the manager have any providers
         * @return bool
         * @since 8.1.0
         */
        bool hasProviders();

        /**
         * Return a preview of a file
         * @param string $file The path to the file where you want a thumbnail from
         * @param int $maxX The maximum X size of the thumbnail. It can be smaller depending on the shape of the image
         * @param int $maxY The maximum Y size of the thumbnail. It can be smaller depending on the shape of the image
         * @param boolean $scaleUp Scale smaller images up to the thumbnail size or not. Might look ugly
         * @return \OCP\IImage
         * @since 6.0.0
         * @deprecated 11 Use getPreview
         */
        IImage createPreview(string file, int maxX = 100, int maxY = 75, bool scaleUp = false);

        /**
         * Returns a preview of a file
         *
         * The cache is searched first and if nothing usable was found then a preview is
         * generated by one of the providers
         *
         * @param File $file
         * @param int $width
         * @param int $height
         * @param bool $crop
         * @param string $mode
         * @param string $mimeType To force a given mimetype for the file (files_versions needs this)
         * @return ISimpleFile
         * @throws NotFoundException
         * @throws \InvalidArgumentException if the preview would be invalid (in case the original image is invalid)
         * @since 11.0.0 - \InvalidArgumentException was added in 12.0.0
         */
        OCP.Files.SimpleFS.ISimpleFile getPreview(File file, int width = -1, int height = -1, bool crop = false, string mode = "fill", string mimeType = null);
        //public ISimpleFile getPreview(File file, int width = -1, int height = -1, bool crop = false, string mode = IPreview::MODE_FILL, string mimeType = null);


        /**
         * Returns true if the passed mime type is supported
         * @param string $mimeType
         * @return boolean
         * @since 6.0.0
         */
        bool isMimeSupported(string mimeType = "*");

        /**
         * Check if a preview can be generated for a file
         *
         * @param \OCP\Files\FileInfo $file
         * @return bool
         * @since 8.0.0
         */
        bool isAvailable(FileInfo file);
    }

}
