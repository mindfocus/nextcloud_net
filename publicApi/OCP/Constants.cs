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
    public static class Constants
    {
        /**
         * CRUDS permissions.
         * @since 8.0.0
         */
        public static int PERMISSION_CREATE => 4;
        public static int PERMISSION_READ => 1;
        public static int PERMISSION_UPDATE => 2;
        public static int PERMISSION_DELETE => 8;
        public static int PERMISSION_SHARE => 16;
        public static int PERMISSION_ALL => 31;

        /**
         * @since 8.0.0 - Updated in 9.0.0 to allow all POSIX chars since we no
         * longer support windows as server platform.
         */
        public static string FILENAME_INVALID_CHARS => "\\/";


        public const string PREVIEW_MODE_FILE = "fill";
        public const string PREVIEW_MODE_COVER = "cover";
    }

}
