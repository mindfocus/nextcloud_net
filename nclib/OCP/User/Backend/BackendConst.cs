namespace OCP.User.Backend
{
    public class BackendConst
    {
        /**
         * actions that user backends can define
         */
        public static int CREATE_USER = 1; // 1 << 0
        public static int SET_PASSWORD = 16; // 1 << 4
        public static int CHECK_PASSWORD = 256; // 1 << 8
        public static int GET_HOME = 4096; // 1 << 12
        public static int GET_DISPLAYNAME = 65536; // 1 << 16
        public static int SET_DISPLAYNAME = 1048576; // 1 << 20
        public static int PROVIDE_AVATAR = 16777216; // 1 << 24
        public static int COUNT_USERS = 268435456; // 1 << 28
    }
}