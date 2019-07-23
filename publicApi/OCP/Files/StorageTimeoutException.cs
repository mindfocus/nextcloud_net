using System;
namespace OCP.Files
{
    /**
     * Storage authentication exception
     * @since 9.0.0
     */
    public class StorageTimeoutException : StorageNotAvailableException
    {

    /**
     * StorageTimeoutException constructor.
     *
     * @param string message
     * @param \Exception|null previous
     * @since 9.0.0
     */
    public StorageTimeoutException(string message = "", Exception previous = null)
    {
        //l = \OC::server.getL10N('core');
        //parent::__construct(l.t('Storage connection timeout. %s', [message]), self::STATUS_TIMEOUT, previous);
    }
}
}
