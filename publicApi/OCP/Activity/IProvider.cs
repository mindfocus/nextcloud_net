using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Activity
{
    /**
     * Interface IProvider
     *
     * @package OCP\Activity
     * @since 11.0.0
     */
    interface IProvider
    {
        /**
         * @param string $language The language which should be used for translating, e.g. "en"
         * @param IEvent $event The current event which should be parsed
         * @param IEvent|null $previousEvent A potential previous event which you can combine with the current one.
         *                                   To do so, simply use setChildEvent($previousEvent) after setting the
         *                                   combined subject on the current event.
         * @return IEvent
         * @throws \InvalidArgumentException Should be thrown if your provider does not know this event
         * @since 11.0.0
         */
        IEvent parse(string language, IEvent currentEvent, IEvent previousEvent = null);
        }

    }
