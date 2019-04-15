namespace publicApi.Log
{
/**
 * Interface IWriter
 *
 * @package OCP\Log
 * @since 14.0.0
 */
    interface IWriter {
        /**
         * @since 14.0.0
         */
        void write(string app, string message, int level);
    }
}