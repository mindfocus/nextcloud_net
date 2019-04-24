using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.FullTextSearch.Service
{
/**
 * Interface IProviderService
 *
 * @since 15.0.0
 *
 * @package OCP\FullTextSearch\Service
 */
    interface IProviderService {


        /**
         * Check if the provider providerId is already indexed.
         *
         * @since 15.0.0
         *
         * @param string providerId
         *
         * @return bool
         */
        bool isProviderIndexed(string providerId);


        /**
         * Add the Javascript API in the navigation page of an app.
         *
         * @since 15.0.0
         */
        void addJavascriptAPI();


    }


}
