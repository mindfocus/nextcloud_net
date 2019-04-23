using System;
namespace OCP.Notification
{
    /**
     * Interface IApp
     *
     * @package OCP\Notification
     * @since 9.0.0
     */
    public interface IApp
    {
        /**
         * @param INotification notification
         * @throws \InvalidArgumentException When the notification is not valid
         * @since 9.0.0
         */
        void notify(INotification notification);

        /**
         * @param INotification notification
         * @since 9.0.0
         */
        void markProcessed(INotification notification);

        /**
         * @param INotification notification
         * @return int
         * @since 9.0.0
         */
        int getCount(INotification notification);
    }
}
