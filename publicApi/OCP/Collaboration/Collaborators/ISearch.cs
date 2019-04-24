using System.Collections;
using System.Collections.Generic;

namespace OCP.Collaboration.Collaborators
{
/**
 * Interface ISearch
 *
 * @package OCP\Collaboration\Collaborators
 * @since 13.0.0
 */
    public interface ISearch {
        /**
         * @param string search
         * @param array shareTypes
         * @param bool lookup
         * @param int limit
         * @param int offset
         * @return array with two elements, 1st ISearchResult as array, 2nd a bool indicating whether more result are available
         * @since 13.0.0
         */
        IList<string> search(string search, IList<string> shareTypes, bool lookup, int limit, int offset);

        /**
         * @param array pluginInfo with keys 'shareType' containing the name of a corresponding constant in \OCP\Share and
         * 	'class' with the class name of the plugin
         * @since 13.0.0
         */
        void registerPlugin(IList<string> pluginInfo);
    }
}