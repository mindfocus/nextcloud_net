using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Lock
{
    /**
     * Class LockedException
     *
     * @package OCP\Lock
     * @since 8.1.0
     */
    class LockedException  : Exception {

	/**
	 * Locked path
	 *
	 * @var string
	 */
	private string path;

	/**
	 * LockedException constructor.
	 *
	 * @param string $path locked path
	 * @param \Exception|null $previous previous exception for cascading
	 * @param string $existingLock since 14.0.0
	 * @since 8.1.0
	 */
	LockedException(string path, Exception previous = null, string? existingLock = null)
    {
            var message = $"{path} is locked";
        if (existingLock != null) {
			message += $", existing lock on file: {existingLock}";
        }
            //parent::__construct($message, 0, $previous);
            //$this->path = $path;
            this.path = path;
    }

    /**
	 * @return string
	 * @since 8.1.0
	 */
    string getPath(){
		return this.path;

    }
}
}
