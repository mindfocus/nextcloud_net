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
            throw new System.NotImplementedException();
        }
    }
}