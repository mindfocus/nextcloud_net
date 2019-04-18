using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar
{
    /**
     * Interface ICalendar
     *
     * @package OCP
     * @since 13.0.0
     */
    interface ICalendar
    {

        /**
         * @return string defining the technical unique key
         * @since 13.0.0
         */
        string getKey();

        /**
         * In comparison to getKey() this function returns a human readable (maybe translated) name
         * @return null|string
         * @since 13.0.0
         */
        string? getDisplayName();

        /**
         * Calendar color
         * @return null|string
         * @since 13.0.0
         */
        string? getDisplayColor();

        /**
         * @param string $pattern which should match within the $searchProperties
         * @param array $searchProperties defines the properties within the query pattern should match
         * @param array $options - optional parameters:
         * 	['timerange' => ['start' => new DateTime(...), 'end' => new DateTime(...)]]
         * @param integer|null $limit - limit number of search results
         * @param integer|null $offset - offset for paging of search results
         * @return array an array of events/journals/todos which are arrays of key-value-pairs
         * @since 13.0.0
         */
        string? search(string pattern, IList<string> searchProperties, IList<string> options, int? limit= null, int? offset= null);

        /**
         * @return integer build up using \OCP\Constants
         * @since 13.0.0
         */
        int getPermissions();
    }

}
