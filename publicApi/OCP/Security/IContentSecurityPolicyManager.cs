using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Security
{
    /**
     * Used for Content Security Policy manipulations
     *
     * @package OCP\Security
     * @since 9.0.0
     */
    interface IContentSecurityPolicyManager
    {
        /**
         * Allows to inject something into the default content policy. This is for
         * example useful when you're injecting Javascript code into a view belonging
         * to another controller and cannot modify its Content-Security-Policy itself.
         * Note that the adjustment is only applied to applications that use AppFramework
         * controllers.
         *
         * To use this from your `app.php` use `\OC::server->getContentSecurityPolicyManager()->addDefaultPolicy(policy)`,
         * policy has to be of type `\OCP\AppFramework\Http\ContentSecurityPolicy`.
         *
         * WARNING: Using this API incorrectly may make the instance more insecure.
         * Do think twice before adding whitelisting resources. Please do also note
         * that it is not possible to use the `disallowXYZ` functions.
         *
         * @param EmptyContentSecurityPolicy policy
         * @since 9.0.0
         */
        void addDefaultPolicy(EmptyContentSecurityPolicy policy);
    }

}
