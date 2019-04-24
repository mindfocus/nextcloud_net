using System;
using System.Collections.Generic;
using System.Text;
using OCP.FullTextSearch.Model;

namespace OCP.FullTextSearch.Service
{
/**
 * Interface ISearchService
 *
 * @since 15.0.0
 *
 * @package OCP\FullTextSearch\Service
 */
    interface ISearchService {


        /**
         * generate a search request, based on an array:
         *
         * request =
         *   [
         *        'providers' =>    (string/array) 'all'
         *        'author' =>       (string) owner of the document.
         *        'search' =>       (string) search string,
         *        'size' =>         (int) number of items to be return
         *        'page' =>         (int) page
         *        'parts' =>        (array) parts of document to search within,
         *        'options' =       (array) search options,
         *        'tags'     =>     (array) tags,
         *        'metatags' =>     (array) metatags,
         *        'subtags'  =>     (array) subtags
         *   ]
         *
         * 'providers' can be an array of providerIds
         *
         * @since 15.0.0
         *
         * @param array request
         *
         * @return ISearchRequest
         */
        ISearchRequest generateSearchRequest(IList<string> request);


        /**
         * Search documents
         *
         * @since 15.0.0
         *
         * @param string userId
         * @param ISearchRequest searchRequest
         *
         * @return ISearchResult[]
         */
        IList<ISearchResult> search(string userId, ISearchRequest searchRequest);

    }


}
