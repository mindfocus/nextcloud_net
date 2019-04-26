using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Activity
{
    /**
     * Interface ISetting
     *
     * @package OCP\Activity
     * @since 11.0.0
     */
    public interface ISetting
    {

        /*
         * @return string Lowercase a-z and underscore only identifier
         * @since 11.0.0
         */
        string getIdentifier();

        /*
         * @return string A translated string
         * @since 11.0.0
         */
        string getName();

        /*
         * @return int whether the filter should be rather on the top or bottom of
         * the admin section. The filters are arranged in ascending order of the
         * priority values. It is required to return a value between 0 and 100.
         * @since 11.0.0
         */
        int getPriority();

        /*
         * @return bool True when the option can be changed for the stream
         * @since 11.0.0
         */
        bool canChangeStream();

        /*
         * @return bool True when the option can be changed for the stream
         * @since 11.0.0
         */
        bool isDefaultEnabledStream();

        /*
         * @return bool True when the option can be changed for the mail
         * @since 11.0.0
         */
        bool canChangeMail();
       
        /*
         * @return bool True when the option can be changed for the stream
         * @since 11.0.0
         */
        bool isDefaultEnabledMail();
    }


}
