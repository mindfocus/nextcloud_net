using System.Collections;

namespace OCP.Log
{
/**
 * Interface IFileBased
 *
 * @package OCP\Log
 *
 * @since 14.0.0
 */
    interface IFileBased {
        /**
         * @since 14.0.0
         */
        string getLogFilePath();

        /**
         * @since 14.0.0
         */
        IList getEntries(int limit=50, int offset=0);
    }

}