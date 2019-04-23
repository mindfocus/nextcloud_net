using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    public interface IUser
    {
        /**
 * get the user id
 *
 * @return string
 * @since 8.0.0
 */
        string getUID();
        /**
 * get the display name for the user, if no specific display name is set it will fallback to the user id
 *
 * @return string
 * @since 8.0.0
 */
        string getDisplayName();
        /**
 * set the display name for the user
 *
 * @param string displayName
 * @return bool
 * @since 8.0.0
 */
        bool setDisplayName(string displayName);
        /**
 * returns the timestamp of the user's last login or 0 if the user did never
 * login
 *
 * @return int
 * @since 8.0.0
 */
        long getLastLogin();
        /**
 * updates the timestamp of the most recent login of this user
 * @since 8.0.0
 */
        bool updateLastLoginTimestamp();
        /**
 * Delete the user
 *
 * @return bool
 * @since 8.0.0
 */
        bool delete();
        /**
 * Set the password of the user
 *
 * @param string password
 * @param string recoveryPassword for the encryption app to reset encryption keys
 * @return bool
 * @since 8.0.0
 */
        bool setPassword(string password, string recoveryPassword = null);
        /**
 * get the users home folder to mount
 *
 * @return string
 * @since 8.0.0
 */
        string getHome();
        /**
 * Get the name of the backend class the user is connected with
 *
 * @return string
 * @since 8.0.0
 */
        string getBackendClassName();
        /**
 * Get the backend for the current user object
 *
 * @return UserInterface
 * @since 15.0.0
 */
        UserInterface getBackend();
        /**
 * check if the backend allows the user to change his avatar on Personal page
 *
 * @return bool
 * @since 8.0.0
 */
        bool canChangeAvatar();
        /**
 * check if the backend supports changing passwords
 *
 * @return bool
 * @since 8.0.0
 */
        bool canChangePassword();
        /**
 * check if the backend supports changing display names
 *
 * @return bool
 * @since 8.0.0
 */
        bool canChangeDisplayName();
        /**
 * check if the user is enabled
 *
 * @return bool
 * @since 8.0.0
 */
        bool isEnabled();
        /**
 * set the enabled status for the user
 *
 * @param bool enabled
 * @since 8.0.0
 */
        void setEnabled(bool enabled = true);
        /**
 * get the users email address
 *
 * @return string|null
 * @since 9.0.0
 */
        string? getEMailAddress();
        /**
 * get the avatar image if it exists
 *
 * @param int size
 * @return IImage|null
 * @since 9.0.0
 */
        IImage? getAvatarImage(int size);
        /**
	 * get the federation cloud id
	 *
	 * @return string
	 * @since 9.0.0
	 */
        string getCloudId();

        /**
         * set the email address of the user
         *
         * @param string|null mailAddress
         * @return void
         * @since 9.0.0
         */
        void setEMailAddress(string? mailAddress);

        /**
         * get the users' quota in human readable form. If a specific quota is not
         * set for the user, the default value is returned. If a default setting
         * was not set otherwise, it is return as 'none', i.e. quota is not limited.
         *
         * @return string
         * @since 9.0.0
         */
        string getQuota();

        /**
         * set the users' quota
         *
         * @param string quota
         * @return void
         * @since 9.0.0
         */
        void setQuota(string quota);
    }
}
