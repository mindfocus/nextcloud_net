using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.AppFramework
{
    /**
     * Class IAppContainer
     * @package OCP\AppFramework
     *
     * This container interface provides short cuts for app developers to access predefined app service.
     * @since 6.0.0
     */
    public interface IAppContainer : IContainer
    {

    /**
	 * used to return the appname of the set application
	 * @return string the name of your application
	 * @since 6.0.0
	 */
    string getAppName();

    /**
	 * @return \OCP\IServerContainer
	 * @since 6.0.0
	 */
    IServerContainer getServer();

    /**
	 * @param string middleWare
	 * @return boolean
	 * @since 6.0.0
	 */
    bool registerMiddleWare(string middleWare);

    /**
	 * Register a capability
	 *
	 * @param string serviceName e.g. 'OCA\Files\Capabilities'
	 * @since 8.2.0
	 */
    void registerCapability(string serviceName);
}
}
