using System;
using System.Collections.Generic;

namespace OCP.Notification
{
    /**
     * Interface IManager
     *
     * @package OCP\Notification
     * @since 9.0.0
     */
    public interface IManager : IApp, INotifier {
    /**
     * @param \Closure service The service must implement IApp, otherwise a
     *                          \InvalidArgumentException is thrown later
     * @since 9.0.0
     */
    void registerApp(Action service);

    /**
     * @param \Closure service The service must implement INotifier, otherwise a
     *                          \InvalidArgumentException is thrown later
     * @param \Closure info    An array with the keys 'id' and 'name' containing
     *                          the app id and the app name
     * @since 9.0.0
     */
    void registerNotifier(Action service, Action info);

    /**
     * @return array App ID => App Name
     * @since 9.0.0
     */
    IDictionary<string,string> listNotifiers();

    /**
     * @return INotification
     * @since 9.0.0
     */
    INotification createNotification();

    /**
     * @return bool
     * @since 9.0.0
     */
    bool hasNotifiers();

    /**
     * @param bool preparingPushNotification
     * @since 14.0.0
     */
    void setPreparingPushNotification(bool preparingPushNotification);

    /**
     * @return bool
     * @since 14.0.0
     */
    bool isPreparingPushNotification();
}

}
