using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Calendar
{
    /**
     * This class provides access to the Nextcloud CalDAV backend.
     * Use this class exclusively if you want to access calendars.
     *
     * Events/Journals/Todos in general will be expressed as an array of key-value-pairs.
     * The keys will match the property names defined in https://tools.ietf.org/html/rfc5545
     *
     * [
     *   'id' => 123,
     *   'type' => 'VEVENT',
     *   'calendar-key' => 42,
     *   'objects' => [
     *     [
     *       'SUMMARY' => ['FooBar', []],
     *       'DTSTART' => ['20171001T123456', ['TZID' => 'EUROPE/BERLIN']],
     *       'DURATION' => ['P1D', []],
     * 	     'ATTENDEE' => [
     *         ['mailto:bla@blub.com', ['CN' => 'Mr. Bla Blub']]
     *       ],
     *       'VALARM' => [
     * 	       [
     *           'TRIGGER' => ['19980101T050000Z', ['VALUE' => DATE-TIME]]
     *         ]
     *       ]
     *     ],
     *   ]
     * ]
     *
     * @since 13.0.0
     */
    public interface IManager
    {

        /**
         * This function is used to search and find objects within the user's calendars.
         * In case $pattern is empty all events/journals/todos will be returned.
         *
         * @param string $pattern which should match within the $searchProperties
         * @param array $searchProperties defines the properties within the query pattern should match
         * @param array $options - optional parameters:
         * 	['timerange' => ['start' => new DateTime(...), 'end' => new DateTime(...)]]
         * @param integer|null $limit - limit number of search results
         * @param integer|null $offset - offset for paging of search results
         * @return array an array of events/journals/todos which are arrays of arrays of key-value-pairs
         * @since 13.0.0
         */
        IDictionary<string,object> search(string pattern, IList<string> searchProperties, IDictionary<string,object> options, int? limit= null, int? offset= null);

        /**
         * Check if calendars are available
         *
         * @return bool true if enabled, false if not
         * @since 13.0.0
         */
        bool isEnabled();

        /**
         * Registers a calendar
         *
         * @param ICalendar $calendar
         * @return void
         * @since 13.0.0
         */
        void registerCalendar(ICalendar calendar);

        /**
         * Unregisters a calendar
         *
         * @param ICalendar $calendar
         * @return void
         * @since 13.0.0
         */
        void unregisterCalendar(ICalendar calendar);

        /**
         * In order to improve lazy loading a closure can be registered which will be called in case
         * calendars are actually requested
         *
         * @param \Closure $callable
         * @return void
         * @since 13.0.0
         */
        //void register(Closure callable);
        void register(Action callable);
        /**
         * @return ICalendar[]
         * @since 13.0.0
         */
        IList<ICalendar> getCalendars();

        /**
         * removes all registered calendar instances
         * @return void
         * @since 13.0.0
         */
        void clear();
    }
}
