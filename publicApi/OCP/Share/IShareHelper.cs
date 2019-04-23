using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.ShareNS
{
    /**
     * Interface IShareHelper
     *
     * @package OCP\Share
     * @since 12
     */
    public interface IShareHelper
    {

        /**
         * @param Node node
         * @return array [ users => [Mapping uid => pathForUser], remotes => [Mapping cloudId => pathToMountRoot]]
         * @since 12
         */
        PhpArray getPathsForAccessList(Files.Node node);
    }

}
