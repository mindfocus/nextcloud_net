namespace OCP.AppFramework.OCS
{
/**
 * Class OCSBadRequestException
 *
 * @package OCP\AppFramework
 * @since 9.1.0
 */
    class OCSBadRequestException : OCSException {
    /**
     * OCSBadRequestException constructor.
     *
     * @param string message
     * @param Exception|null previous
     * @since 9.1.0
     */
    public OCSBadRequestException(string message = "", System.Exception previous = null) {
        parent::__construct(message, Http::STATUS_BAD_REQUEST, previous);
    }

    }

}