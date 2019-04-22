using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace OCP
{
    /**
     * This class provides avatar functionality
     * @since 6.0.0
     */
    interface IAvatar
    {

        /**
         * get the users avatar
         * @param int size size in px of the avatar, avatars are square, defaults to 64, -1 can be used to not scale the image
         * @return boolean|\OCP\IImage containing the avatar or false if there's no image
         * @since 6.0.0 - size of -1 was added in 9.0.0
         */
        IImage? get(int size = 64);

        /**
         * Check if an avatar exists for the user
         *
         * @return bool
         * @since 8.1.0
         */
        bool exists();

        /**
         * Check if the avatar of a user is a custom uploaded one
         *
         * @return bool
         * @since 14.0.0
         */
        bool isCustomAvatar();

	/**
	 * sets the users avatar
	 * @param \OCP\IImage|resource|string data An image object, imagedata or path to set a new avatar
	 * @throws \Exception if the provided file is not a jpg or png image
	 * @throws \Exception if the provided image is not valid
	 * @throws \OC\NotSquareException if the image is not square
	 * @return void
	 * @since 6.0.0
	 */
	void set(IImage data);

        /**
         * remove the users avatar
         * @return void
         * @since 6.0.0
         */
        void remove();

        /**
         * Get the file of the avatar
         * @param int size -1 can be used to not scale the image
         * @return File
         * @throws NotFoundException
         * @since 9.0.0
         */
        File getFile(int size);

        /**
         * @param string text
         * @return Color Object containting r g b int in the range [0, 255]
         * @since 14.0.0
         */
        Color avatarBackgroundColor(string text);

        /**
         * Handle a changed user
         * @since 13.0.0
         */
        void userChanged(string feature, object oldValue, object newValue);
    }

}
