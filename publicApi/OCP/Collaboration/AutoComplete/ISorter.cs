using System.Collections.Generic;

namespace OCP.Collaboration.AutoComplete
{
/**
 * Interface ISorter
 *
 * Sorts the list of .e.g users for auto completion
 *
 * @package OCP\Collaboration\AutoComplete
 * @since 13.0.0
 */
    public interface ISorter {

        /**
         * @return string The ID of the sorter, e.g. commenters
         * @since 13.0.0
         */
        string getId();

        /**
         * executes the sort action
         *
         * @param array sortArray the array to be sorted, provided as reference
         * @param array context carries key 'itemType' and 'itemId' of the source object (e.g. a file)
         * @since 13.0.0
         */
        void sort(IList<string> sortArray, IList<string> context);
    }

}