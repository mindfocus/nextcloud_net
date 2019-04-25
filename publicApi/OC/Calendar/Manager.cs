using System;
using System.Collections.Generic;
using System.Text;
using OCP.Calendar;

namespace OC.Calendar
{
public class Manager : IManager {

	/**
	 * @var ICalendar[] holds all registered calendars
	 */
	private IList<ICalendar> calendars=new List<ICalendar>();

	/**
	 * @var \Closure[] to call to load/register calendar providers
	 */
	private IList<Action> calendarLoaders=new List<Action>();

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
	public IList<string> search(string pattern, IList<string> searchProperties, IList<string> options, int? limit=null, int? offset=null) {
        this.loadCalendars();
        foreach (var calendar in this.calendars)
        {
            var r = calendar.search(pattern, searchProperties, options, limit, offset);
            foreach (var o in r)
            {
                
            }
        }
		$result = [];
		foreach($this->calendars as $calendar) {
			$r = $calendar->search($pattern, $searchProperties, $options, $limit, $offset);
			foreach($r as $o) {
				$o['calendar-key'] = $calendar->getKey();
				$result[] = $o;
			}
		}

		return $result;
	}

	/**
	 * Check if calendars are available
	 *
	 * @return bool true if enabled, false if not
	 * @since 13.0.0
	 */
	public function isEnabled() {
		return !empty($this->calendars) || !empty($this->calendarLoaders);
	}

	/**
	 * Registers a calendar
	 *
	 * @param ICalendar $calendar
	 * @return void
	 * @since 13.0.0
	 */
	public function registerCalendar(ICalendar $calendar) {
		$this->calendars[$calendar->getKey()] = $calendar;
	}

	/**
	 * Unregisters a calendar
	 *
	 * @param ICalendar $calendar
	 * @return void
	 * @since 13.0.0
	 */
	public function unregisterCalendar(ICalendar $calendar) {
		unset($this->calendars[$calendar->getKey()]);
	}

	/**
	 * In order to improve lazy loading a closure can be registered which will be called in case
	 * calendars are actually requested
	 *
	 * @param \Closure $callable
	 * @return void
	 * @since 13.0.0
	 */
	public function register(\Closure $callable) {
		$this->calendarLoaders[] = $callable;
	}

	/**
	 * @return ICalendar[]
	 * @since 13.0.0
	 */
	public function getCalendars() {
		$this->loadCalendars();

		return array_values($this->calendars);
	}

	/**
	 * removes all registered calendar instances
	 * @return void
	 * @since 13.0.0
	 */
	public function clear() {
		$this->calendars = [];
		$this->calendarLoaders = [];
	}

	/**
	 * loads all calendars
	 */
	private function loadCalendars() {
		foreach($this->calendarLoaders as $callable) {
			$callable($this);
		}
		$this->calendarLoaders = [];
	}
}

}
