using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Remote
{
    /**
     * User info for a remote user
     *
     * @since 13.0.0
     */
    public interface IUser
    {
        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getUserId();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getEmail();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getDisplayName();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getPhone();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getAddress();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getWebsite();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getTwitter();

        /**
         * @return string[]
         *
         * @since 13.0.0
         */
        IList<string > getGroups();

        /**
         * @return string
         *
         * @since 13.0.0
         */
        string getLanguage();

        /**
         * @return int
         *
         * @since 13.0.0
         */
        int getUsedSpace();

        /**
         * @return int
         *
         * @since 13.0.0
         */
        int getFreeSpace();

        /**
         * @return int
         *
         * @since 13.0.0
         */
        int getTotalSpace();

        /**
         * @return int
         *
         * @since 13.0.0
         */
        int getQuota();
    }

}
