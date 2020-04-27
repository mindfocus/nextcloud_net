namespace OCP.AppFramework.OCS
{
/**
 * Class OCSNotFoundException
 *
 * @package OCP\AppFramework
 * @since 9.1.0
 */
    class OCSNotFoundException extends OCSException {
    /**
     * OCSNotFoundException constructor.
     *
     * @param string message
     * @param Exception|null previous
     * @since 9.1.0
     */
    public OCSNotFoundException(string message = "", Exception previous = null) {
        parent::__construct(message, Http::STATUS_NOT_FOUND, previous);
    }
    }

}