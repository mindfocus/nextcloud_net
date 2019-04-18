using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi
{
    public interface IImage
    {
        /**
 * Determine whether the object contains an image resource.
 *
 * @return bool
 * @since 8.1.0
 */
        bool valid();
        /**
 * Returns the MIME type of the image or an empty string if no image is loaded.
 *
 * @return string
 * @since 8.1.0
 */
        string mimeType();
        /**
 * Returns the width of the image or -1 if no image is loaded.
 *
 * @return int
 * @since 8.1.0
 */
        int width();
        /**
 * Returns the height of the image or -1 if no image is loaded.
 *
 * @return int
 * @since 8.1.0
 */
        int height();
        /**
 * Returns the width when the image orientation is top-left.
 *
 * @return int
 * @since 8.1.0
 */
        int widthTopLeft();
        /**
 * Returns the height when the image orientation is top-left.
 *
 * @return int
 * @since 8.1.0
 */
        int heightTopLeft();
        /**
 * Outputs the image.
 *
 * @param string $mimeType
 * @return bool
 * @since 8.1.0
 */
        int show(string mimeType = null);

        /**
         * Saves the image.
         *
         * @param string $filePath
         * @param string $mimeType
         * @return bool
         * @since 8.1.0
         */
        bool save(string filePath = null, string mimeType = null);

        /**
         * @return resource Returns the image resource in any.
         * @since 8.1.0
         */
        object resource();

        /**
         * @return string Returns the raw data mimetype
         * @since 13.0.0
         */
        object dataMimeType();

        /**
         * @return string Returns the raw image data.
         * @since 8.1.0
         */
        object  data();

        /**
         * (I'm open for suggestions on better method name ;)
         * Get the orientation based on EXIF data.
         *
         * @return int The orientation or -1 if no EXIF data is available.
         * @since 8.1.0
         */
        int getOrientation();

        /**
         * (I'm open for suggestions on better method name ;)
         * Fixes orientation based on EXIF data.
         *
         * @return bool
         * @since 8.1.0
         */
        bool fixOrientation();

        /**
         * Resizes the image preserving ratio.
         *
         * @param integer $maxSize The maximum size of either the width or height.
         * @return bool
         * @since 8.1.0
         */
        bool resize(int maxSize);

        /**
         * @param int $width
         * @param int $height
         * @return bool
         * @since 8.1.0
         */
        bool preciseResize(int width, int height);

	/**
	 * Crops the image to the middle square. If the image is already square it just returns.
	 *
	 * @param int $size maximum size for the result (optional)
	 * @return bool for success or failure
	 * @since 8.1.0
	 */
	bool centerCrop(int size = 0);

        /**
         * Crops the image from point $x$y with dimension $wx$h.
         *
         * @param int $x Horizontal position
         * @param int $y Vertical position
         * @param int $w Width
         * @param int $h Height
         * @return bool for success or failure
         * @since 8.1.0
         */
        bool  crop(int x, int y, int w, int h);

	/**
	 * Resizes the image to fit within a boundary while preserving ratio.
	 *
	 * @param integer $maxWidth
	 * @param integer $maxHeight
	 * @return bool
	 * @since 8.1.0
	 */
	bool fitIn(string maxWidth, string maxHeight);

        /**
         * Shrinks the image to fit within a boundary while preserving ratio.
         *
         * @param integer $maxWidth
         * @param integer $maxHeight
         * @return bool
         * @since 8.1.0
         */
        bool scaleDownToFit(int maxWidth, int maxHeight);
    }
}
