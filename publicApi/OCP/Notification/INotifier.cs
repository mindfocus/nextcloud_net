using System;
namespace OCP.Notification
{
    /**
     * Interface INotifier
     *
     * @package OCP\Notification
     * @since 9.0.0
     */
    public interface INotifier
    {
        /**
         * @param INotification notification
         * @param string languageCode The code of the language that should be used to prepare the notification
         * @return INotification
         * @throws \InvalidArgumentException When the notification was not prepared by a notifier
         * @since 9.0.0
         */
        INotification prepare(INotification notification, string languageCode);
    }

}
