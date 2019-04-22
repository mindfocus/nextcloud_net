using System;
namespace OCP.Files
{
    /**
     * Storage authentication exception
     * @since 9.0.0
     */
    class StorageConnectionException : StorageNotAvailableException
    {

    /**
     * StorageConnectionException constructor.
     *
     * @param string message
     * @param \Exception|null previous
     * @since 9.0.0
     */
    public StorageConnectionException(string message = "", Exception previous = null)
    {
        //l = \OC::server->getL10N('core');
        //parent::__construct(l->t('Storage connection error. %s', [message]), self::STATUS_NETWORK_ERROR, previous);
    }
}

}
