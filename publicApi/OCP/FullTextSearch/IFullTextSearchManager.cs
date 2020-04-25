using System;
using OCP.FullTextSearch.Model;
using OCP.FullTextSearch.Service;

namespace OCP.FullTextSearch
{
    /**
     * Interface IFullTextSearchManager
     *
     * Should be used to manage FullTextSearch from the app that contains your
     * Content Provider/Search Platform.
     *
     * @since 15.0.0
     *
     * @package OCP\FullTextSearch
     */
    interface IFullTextSearchManager
    {


        /**
         * Register a IProviderService.
         *
         * @since 15.0.0
         *
         * @param IProviderService providerService
         */
        public void registerProviderService(IProviderService providerService);

        /**
         * Register a IIndexService.
         *
         * @since 15.0.0
         *
         * @param IIndexService indexService
         */
        public void registerIndexService(IIndexService indexService);

        /**
         * Register a ISearchService.
         *
         * @since 15.0.0
         *
         * @param ISearchService searchService
         */
        public void registerSearchService(ISearchService searchService);


        /**
         * Add the Javascript API in the navigation page of an app.
         * Needed to replace the default search.
         *
         * @since 15.0.0
         */
        public void addJavascriptAPI();


        /**
         * Check if the provider providerId is already indexed.
         *
         * @since 15.0.0
         *
         * @param string providerId
         *
         * @return bool
         */
        public bool isProviderIndexed(string providerId);


    /**
     * Retrieve an Index from the database, based on the Id of the Provider
     * and the Id of the Document
     *
     * @since 15.0.0
     *
     * @param string providerId
     * @param string documentId
     *
     * @return IIndex
     */
    public IIndex getIndex(string providerId, string documentId);


    /**
     * Create a new Index.
     *
     * This method must be called when a new document is created.
     *
     * @since 15.0.0
     *
     * @param string providerId
     * @param string documentId
     * @param string userId
     * @param int status
     *
     * @return IIndex
     */
    public IIndex createIndex(string providerId, string documentId, string userId, int status = 0);


    /**
     * Update the status of an Index. status is a bitflag, setting reset to
     * true will reset the status to the value defined in the parameter.
     *
     * @since 15.0.0
     *
     * @param string providerId
     * @param string documentId
     * @param int status
     * @param bool reset
     */
    public void updateIndexStatus(string providerId, string documentId, int status, bool reset = false);


        /**
         * Update the status of an array of Index. status is a bit flag, setting reset to
         * true will reset the status to the value defined in the parameter.
         *
         * @since 15.0.0
         *
         * @param string providerId
         * @param array documentIds
         * @param int status
         * @param bool reset
         */
        public void updateIndexesStatus(string providerId, array documentIds, int status, bool reset = false);

        /**
         * Update an array of Index.
         *
         * @since 15.0.0
         *
         * @param IIndex[] indexes
         */
        public void updateIndexes(array indexes);

        /**
         * Search using an array as request. If userId is empty, will use the
         * current session.
         *
         * @see ISearchService::generateSearchRequest
         *
         * @since 15.0.0
         *
         * @param array request
         * @param string userId
         * @return ISearchResult[]
         */
        public array search(array request, string userId = '');


}


}
