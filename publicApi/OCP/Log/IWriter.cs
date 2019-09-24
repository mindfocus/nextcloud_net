namespace OCP.Log
{
/**
 * Interface IWriter
 *
 * @package OCP\Log
 * @since 14.0.0
 */
    public interface IWriter {
        /**
         * @since 14.0.0
         */
        void write(string app, string message, int level);
    }

}