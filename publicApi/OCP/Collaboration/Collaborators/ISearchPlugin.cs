namespace OCP.Collaboration.Collaborators
{
/**
 * Interface ISearchPlugin
 *
 * @package OCP\Collaboration\Collaborators
 * @since 13.0.0
 */
    public interface ISearchPlugin {
        /**
         * @param string search
         * @param int limit
         * @param int offset
         * @param ISearchResult searchResult
         * @return bool whether the plugin has more results
         * @since 13.0.0
         */
        bool search(string search, int limit, int offset, ISearchResult searchResult);
    }

}