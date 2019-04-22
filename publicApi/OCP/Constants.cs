using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Class Constants
     *
     * @package OCP
     * @since 8.0.0
     */
    class Constants
    {
        /**
         * CRUDS permissions.
         * @since 8.0.0
         */
        int PERMISSION_CREATE = 4;
        int PERMISSION_READ = 1;
        int PERMISSION_UPDATE = 2;
        int PERMISSION_DELETE = 8;
        int PERMISSION_SHARE = 16;
        int PERMISSION_ALL = 31;

        /**
         * @since 8.0.0 - Updated in 9.0.0 to allow all POSIX chars since we no
         * longer support windows as server platform.
         */
        string FILENAME_INVALID_CHARS = "\\/";
    }

}
