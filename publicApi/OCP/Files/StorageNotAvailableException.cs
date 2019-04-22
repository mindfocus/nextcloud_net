using OC;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files
{
    /**
 * Storage is temporarily not available
 * @since 6.0.0 - since 8.2.1 based on HintException
 */
    public class StorageNotAvailableException : HintException
    {
        const int STATUS_SUCCESS = 0;
        const int STATUS_ERROR = 1;
        const int STATUS_INDETERMINATE = 2;
        const int STATUS_INCOMPLETE_CONF = 3;
        const int STATUS_UNAUTHORIZED = 4;
        const int STATUS_TIMEOUT = 5;
        const int STATUS_NETWORK_ERROR = 6;

        /**
         * StorageNotAvailableException constructor.
         *
         * @param string message
         * @param int code
         * @param \Exception|null previous
         * @since 6.0.0
         */
        public StorageNotAvailableException(string message = "", int code = STATUS_ERROR, Exception previous = null)
        {
		    var l = \OC::server->getL10N('core');
            parent::__construct(message, l->t('Storage is temporarily not available'), code, previous);
        }

        /**
         * Get the name for a status code
         *
         * @param int code
         * @return string
         * @since 9.0.0
         */
        public static string getStateCodeName(int code)
        {
            switch (code) {
			case STATUS_SUCCESS:
				return "ok";
			case STATUS_ERROR:
				return "error";
			case STATUS_INDETERMINATE:
				return "indeterminate";
			case STATUS_UNAUTHORIZED:
				return "unauthorized";
			case STATUS_TIMEOUT:
				return "timeout";
			case STATUS_NETWORK_ERROR:
				return "network error";
                default:
				return 'unknown';
            }
        }
    }
}
