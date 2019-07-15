using System;
using System.Collections.Generic;

namespace publicApi.OCP.SystemTag
{
    /**
     * Class MapperEvent
     *
     * @package OCP\SystemTag
     * @since 9.0.0
     */
    class MapperEvent : ext.Event
    {

    const string EVENT_ASSIGN = @"OCP\\SystemTag\\ISystemTagObjectMapper::assignTags";
    const string EVENT_UNASSIGN = @"OCP\\SystemTag\\ISystemTagObjectMapper::unassignTags";

    /** @var string */
    protected string @event;
    /** @var string */
    protected string objectType;
    /** @var string */
    protected string objectId;
    /** @var int[] */
    protected IList<int> tags;

    /**
     * DispatcherEvent constructor.
     *
     * @param string event
     * @param string objectType
     * @param string objectId
     * @param int[] tags
     * @since 9.0.0
     */
    public MapperEvent(string @event, string objectType, string objectId, IList<int> tags)
    {
            this.@event = @event;
            this.objectType = objectType;
            this.objectId = objectId;
            this.tags = tags;
    }

    /**
     * @return string
     * @since 9.0.0
     */
    public string getEvent() {
        return this.@event;
    }

    /**
     * @return string
     * @since 9.0.0
     */
    public string getObjectType() {
        return this.objectType;
    }

    /**
     * @return string
     * @since 9.0.0
     */
    public string getObjectId() {
        return this.objectId;
    }

    /**
     * @return int[]
     * @since 9.0.0
     */
    public IList<int> getTags() {
        return this.tags;
    }
}

}
