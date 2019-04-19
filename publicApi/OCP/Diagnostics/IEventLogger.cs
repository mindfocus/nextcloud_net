using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Diagnostics
{
    /**
     * Interface IEventLogger
     *
     * @package OCP\Diagnostics
     * @since 8.0.0
     */
    public interface IEventLogger
    {
        /**
         * Mark the start of an event setting its ID $id and providing event description $description.
         *
         * @param string $id
         * @param string $description
         * @since 8.0.0
         */
        void start(string id, string description);

        /**
         * Mark the end of an event with specific ID $id, marked by start() method.
         * Ending event should store \OCP\Diagnostics\IEvent to
         * be returned with getEvents() method.
         *
         * @param string $id
         * @since 8.0.0
         */
        void end(string id);

        /**
         * Mark the start and the end of an event with specific ID $id and description $description,
         * explicitly marking start and end of the event, represented by $start and $end timestamps.
         * Logging event should store \OCP\Diagnostics\IEvent to
         * be returned with getEvents() method.
         *
         * @param string $id
         * @param string $description
         * @param float $start
         * @param float $end
         * @since 8.0.0
         */
        void log(string id, string description, float start, float end);

        /**
         * This method should return all \OCP\Diagnostics\IEvent objects stored using
         * start()/end() or log() methods
         *
         * @return \OCP\Diagnostics\IEvent[]
         * @since 8.0.0
         */
        IList<IEvent> getEvents();

        /**
         * Activate the module for the duration of the request. Deactivated module
         * does not create and store \OCP\Diagnostics\IEvent objects.
         * Only activated module should create and store objects to be
         * returned with getEvents() call.
         *
         * @since 12.0.0
         */
        void activate();
    }

}
