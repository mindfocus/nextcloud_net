using  System;
using System.Collections;
using System.Collections.Generic;

namespace OCP.Collaboration.Collaborators
{
/**
 * Interface ISearchResult
 *
 * @package OCP\Collaboration\Collaborators
 * @since 13.0.0
 */
    public interface ISearchResult {
        /**
         * @param SearchResultType type
         * @param array matches
         * @param array|null exactMatches
         * @since 13.0.0
         */
        void addResultSet(SearchResultType type, IList<string> matches, IList<string> exactMatches = null);

        /**
         * @param SearchResultType type
         * @param string collaboratorId
         * @return bool
         * @since 13.0.0
         */
        bool hasResult(SearchResultType type, string collaboratorId);

        /**
         * @param SearchResultType type
         * @since 13.0.0
         */
        void unsetResult(SearchResultType type);

        /**
         * @param SearchResultType type
         * @since 13.0.0
         */
        void markExactIdMatch(SearchResultType type);

        /**
         * @param SearchResultType type
         * @return bool
         * @since 13.0.0
         */
        bool hasExactIdMatch(SearchResultType type);

        /**
         * @return array
         * @since 13.0.0
         */
        IList<string> asArray();
    }

}