using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Share
{
    /**
     * Interface IProviderFactory
     *
     * @package OC\Share20
     * @since 9.0.0
     */
    public interface IProviderFactory
    {

        /**
         * IProviderFactory constructor.
         * @param IServerContainer serverContainer
         * @since 9.0.0
         */
        //public function __construct(IServerContainer serverContainer);

        /**
         * @param string id
         * @return IShareProvider
         * @throws ProviderException
         * @since 9.0.0
         */
        IShareProvider getProvider(string id);

        /**
         * @param int shareType
         * @return IShareProvider
         * @throws ProviderException
         * @since 9.0.0
         */
        IShareProvider getProviderForType(int shareType);

        /**
         * @return IShareProvider[]
         * @since 11.0.0
         */
        IList<IShareProvider> getAllProviders();
    }

}
