using OC;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ShareNS.Exceptions
{
    /**
 * Class GenericEncryptionException
 *
 * @package OCP\Share\Exceptions
 * @since 9.0.0
 */
    class GenericShareException : HintException
    {
        /**
 * @param string message
 * @param string hint
 * @param int code
 * @param \Exception|null previous
 * @since 9.0.0
 */
        public GenericShareException(string message = "", string hint = "", int code = 0, Exception previous = null)
        {
            //if (empty(message))
            //{
            //    message = 'There was an error retrieving the share. Maybe the link is wrong, it was unshared, or it was deleted.';
            //}
            //parent::__construct(message, hint, code, previous);
        }
    }
}
