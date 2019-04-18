using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Small Interface for Search
     * @since 7.0.0
     */
    interface ISearch
    {

        /**
         * Search all providers for $query
         * @param string $query
         * @param string[] $inApps optionally limit results to the given apps
         * @param int $page pages start at page 1
         * @param int $size
         * @return array An array of OCP\Search\Result's
         * @since 8.0.0
         */
        IList<OCP.Search.Result> searchPaged(string query, IList<string> inApps , int page = 1, int size = 30);

        /**
         * Register a new search provider to search with
         * @param string $class class name of a OCP\Search\Provider
         * @param array $options optional
         * @since 7.0.0
         */
        void registerProvider(string classp, IList<string> options );

	/**
	 * Remove one existing search provider
	 * @param string $provider class name of a OCP\Search\Provider
	 * @since 7.0.0
	 */
	void removeProvider(OCP.Search.Provider provider);

        /**
         * Remove all registered search providers
         * @since 7.0.0
         */
        void clearProviders();

    }

}
