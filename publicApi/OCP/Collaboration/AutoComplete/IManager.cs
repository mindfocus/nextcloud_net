using System.Collections;
using System.Collections.Generic;

namespace OCP.Collaboration.AutoComplete
{
/**
 * Interface IManager
 *
 * @package OCP\Collaboration\AutoComplete
 * @since 13.0.0
 */
    interface IManager {
        /**
         * @param string className – class name of the ISorter implementation
         * @since 13.0.0
         */
        void registerSorter(string className);

        /**
         * @param array sorters	list of sorter IDs, seperated by "|"
         * @param array sortArray	array representation of OCP\Collaboration\Collaborators\ISearchResult
         * @param array context	context info of the search, keys: itemType, itemId
         * @since 13.0.0
         */
        void runSorters(IList<string> sorters, IList<string> sortArray, IList<string> context);
    }

}