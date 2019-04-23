using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Settings
{
    /**
     * @since 9.1
     */
    interface ISettings
    {

        /**
         * @return TemplateResponse returns the instance with all parameters set, ready to be rendered
         * @since 9.1
         */
        TemplateResponse getForm();

        /**
         * @return string the section ID, e.g. 'sharing'
         * @since 9.1
         */
        string getSection();

        /**
         * @return int whether the form should be rather on the top or bottom of
         * the admin section. The forms are arranged in ascending order of the
         * priority values. It is required to return a value between 0 and 100.
         *
         * E.g.: 70
         * @since 9.1
         */
        int getPriority();
    }

}
