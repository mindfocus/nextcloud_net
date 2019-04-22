using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Encryption
{
    /**
 * Class GenericEncryptionException
 *
 * @package OCP\Encryption\Exceptions
 * @since 8.1.0
 */
class GenericEncryptionException : HintException {

	/**
	 * @param string message
	 * @param string hint
	 * @param int code
	 * @param \Exception|null previous
	 * @since 8.1.0
	 */
	public GenericEncryptionException(string message = "", string hint = "", int code = 0, System.Exception previous = null)
     {
		// if (message == "") {
			// message = "Unspecified encryption exception";
		// }
		// parent::__construct(message, hint, code, previous);
	}

}

}