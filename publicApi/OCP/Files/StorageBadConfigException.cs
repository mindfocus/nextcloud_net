using System;
namespace OCP.Files
{
    /**
     * Storage has bad or missing config params
     * @since 9.0.0
     */
    public class StorageBadConfigException : StorageNotAvailableException
    {

    /**
     * ExtStorageBadConfigException constructor.
     *
     * @param string message
     * @param \Exception|null previous
     * @since 9.0.0
     */
    public StorageBadConfigException(string message = "", Exception previous = null)
    {
        //l = \OC::server->getL10N('core');
        //parent::__construct(l->t('Storage incomplete configuration. %s', [message]), self::STATUS_INCOMPLETE_CONF, previous);
    }

}

}
