using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ext;
using OCP;
using OCP.Activity;

namespace OC.Activity
{
    public class EventMerger : IEventMerger
    {
        private IL10N _l10n;

        public EventMerger(IL10N l10n)
        {
            this._l10n = l10n;
        }
        public IEvent mergeEvents(string mergeParameter, IEvent @event, IEvent previousEvent = null)
        {
            if (previousEvent == null)
            {
                return @event;
            }

            if (@event.getApp() != previousEvent.getApp())
            {
                return @event;
            }

            if (@event.getMessage().IsNotEmpty() || previousEvent.getMessage().IsNotEmpty())
            {
                return @event;
            }

            if (@event.getSubject() != previousEvent.getSubject())
            {
                return @event;
            }

            if (Math.Abs(@event.getTimestamp() - previousEvent.getTimestamp()) > 3 * 60 * 60)
            {
                return @event;
            }

            return @event;
        }
        /**
	 * @param array[] $parameters
	 * @param string $mergeParameter
	 * @param array $parameter
	 * @return bool
	 */
        protected bool checkParameterAlreadyExits(IDictionary<string,string> parameters, string mergeParameter, string parameter) {
            foreach (var param in parameters) {
                if (Regex.IsMatch(param.Key, @"/^" +  mergeParameter +  @"(\d+)?$/" )) {
                    if (param.Value == parameter) {
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * @param string $mergeParameter
         * @param IEvent $event
         * @param IEvent $previousEvent
         * @return array
         * @throws \UnexpectedValueException
         */
        protected IDictionary<string,string> combineParameters(string mergeParameter, IEvent @event, IEvent previousEvent)
        {
            return null;
        }
    }
}