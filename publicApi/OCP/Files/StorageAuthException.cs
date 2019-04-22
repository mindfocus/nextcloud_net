using System;
namespace OCP.Files
{
    /**
     * Storage authentication exception
     * @since 9.0.0
     */
    public class StorageAuthException : StorageNotAvailableException
    {

    /**
     * StorageAuthException constructor.
     *
     * @param string message
     * @param \Exception|null previous
     * @since 9.0.0
     */
    public StorageAuthException(string message = "", Exception previous = null)
    {
        //l = \OC::server->getEventetL10N('core');
        //parent::__construct(l->t('Storage unauthorized. %s', [message]), STATUS_UNAUTHORIZED, previous);
    }
}

}
