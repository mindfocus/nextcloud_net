using System;
using System.Collections.Generic;

namespace OCP.FullTextSearch.Model
{
    /**
     * Interface ISearchRequest
     *
     * When a search request is initiated, from a request from the front-end or using
     * the IFullTextSearchManager::search() method, FullTextSearch will create a
     * SearchRequest object, based on this interface.
     *
     * The object will be passed to the targeted Content Provider so it can convert
     * search options using available method.
     *
     * The object is then encapsulated in a SearchResult and send to the
     * Search Platform.
     *
     * @since 15.0.0
     *
     *
     * @package OCP\FullTextSearch\Model
     */
    interface ISearchRequest
    {


        /**
         * Get the maximum number of results to be returns by the Search Platform.
         *
         * @since 15.0.0
         *
         * @return int
         */
        int getSize();


        /**
         * Get the current page.
         * Used by pagination.
         *
         * @since 15.0.0
         *
         * @return int
         */
        int getPage();


        /**
         * Get the author of the request.
         *
         * @since 15.0.0
         *
         * @return string
         */
        string getAuthor();

        /**
         * Get the searched string.
         *
         * @since 15.0.0
         *
         * @return string
         */
        string getSearch();


        /**
         * Get the value of an option (as string).
         *
         * @since 15.0.0
         *
         * @param string option
         * @param string default
         *
         * @return string
         */
        string getOption(string option, string @default = "");

    /**
     * Get the value of an option (as array).
     *
     * @since 15.0.0
     *
     * @param string option
     * @param array default
     *
     * @return array
     */
    IList<string> getOptionArray(string option, IList<string> @default);


        /**
         * Limit the search to a part of the document.
         *
         * @since 15.0.0
         *
         * @param string part
         *
         * @return ISearchRequest
         */
        ISearchRequest addPart(string part) ;

        /**
         * Limit the search to an array of parts of the document.
         *
         * @since 15.0.0
         *
         * @param array parts
         *
         * @return ISearchRequest
         */
        ISearchRequest setParts(IList<string> parts) ;

        /**
         * Get the parts the search is limited to.
         *
         * @since 15.0.0
         *
         * @return array
         */
        IList<string> getParts();


        /**
         * Limit the search to a specific meta tag.
         *
         * @since 15.0.0
         *
         * @param string tag
         *
         * @return ISearchRequest
         */
        ISearchRequest addMetaTag(string tag);

    /**
     * Get the meta tags the search is limited to.
     *
     * @since 15.0.0
     *
     * @return array
     */
    IList<string> getMetaTags();

        /**
         * Limit the search to an array of meta tags.
         *
         * @since 15.0.0
         *
         * @param array tags
         *
         * @return ISearchRequest
         */
        ISearchRequest setMetaTags(IList<string> tags) ;


        /**
         * Limit the search to a specific sub tag.
         *
         * @since 15.0.0
         *
         * @param string source
         * @param string tag
         *
         * @return ISearchRequest
         */
        ISearchRequest addSubTag(string source, string tag);

    /**
     * Get the sub tags the search is limited to.
     *
     * @since 15.0.0
     *
     * @param bool formatted
     *
     * @return array
     */
    IList<string> getSubTags(bool formatted);

        /**
         * Limit the search to an array of sub tags.
         *
         * @since 15.0.0
         *
         * @param array tags
         *
         * @return ISearchRequest
         */
        ISearchRequest setSubTags(IList<string> tags) ;


        /**
         * Limit the search to a specific field of the mapping, using a full string.
         *
         * @since 15.0.0
         *
         * @param string field
         *
         * @return ISearchRequest
         */
        ISearchRequest addLimitField(string field) ;

    /**
     * Get the fields the search is limited to.
     *
     * @since 15.0.0
     *
     * @return array
     */
    IList<string> getLimitFields();


        /**
         * Limit the search to a specific field of the mapping, using a wildcard on
         * the search string.
         *
         * @since 15.0.0
         *
         * @param string field
         *
         * @return ISearchRequest
         */
        ISearchRequest addWildcardField(string field);

    /**
     * Get the limit to field of the mapping.
     *
     * @since 15.0.0
     *
     * @return array
     */
    IList<string> getWildcardFields();


        /**
         * Filter the results, based on a group of field, using regex
         *
         * @since 15.0.0
         *
         * @param array filters
         *
         * @return ISearchRequest
         */
        ISearchRequest addRegexFilters(IList<string> filters) ;

        /**
         * Get the regex filters the search is limit to.
         *
         * @since 15.0.0
         *
         * @return array
         */
        IList<string> getRegexFilters();


        /**
         * Filter the results, based on a group of field, using wildcard
         *
         * @since 15.0.0
         *
         * @param array filter
         *
         * @return ISearchRequest
         */
        ISearchRequest addWildcardFilter(IList<string> filter);

        /**
         * Get the wildcard filters the search is limit to.
         *
         * @since 15.0.0
         *
         * @return array
         */
        IList<string> getWildcardFilters();


        /**
         * Add an extra field to the search.
         *
         * @since 15.0.0
         *
         * @param string field
         *
         * @return ISearchRequest
         */
        ISearchRequest addField(string field);

    /**
     * Get the list of extra field to search into.
     *
     * @since 15.0.0
     *
     * @return array
     */
    IList<string> getFields();


}


}
