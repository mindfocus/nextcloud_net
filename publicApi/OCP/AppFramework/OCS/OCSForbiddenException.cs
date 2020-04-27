namespace OCP.AppFramework.OCS
{
/**
 * Class OCSForbiddenException
 *
 * @package OCP\AppFramework
 * @since 9.1.0
 */
    class OCSForbiddenException extends OCSException {
    /**
     * OCSForbiddenException constructor.
     *
     * @param string message
     * @param Exception|null previous
     * @since 9.1.0
     */
    public OCSForbiddenException(string message = "", Exception previous = null) {
        parent::__construct(message, Http::STATUS_FORBIDDEN, previous);
    }
    }

}