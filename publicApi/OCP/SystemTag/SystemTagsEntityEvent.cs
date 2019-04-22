using System;
using System.Collections.Generic;

namespace OCP.SystemTag
{
    /**
     * Class SystemTagsEntityEvent
     *
     * @package OCP\SystemTag
     * @since 9.1.0
     */
    class SystemTagsEntityEvent : ext.Event
    {

    const string EVENT_ENTITY = @"OCP\SystemTag\ISystemTagManager::registerEntity";

    /** @var string */
    protected string @event;
    /** @var \Closure[] */
    protected IDictionary<string,Action> collections;

    /**
     * SystemTagsEntityEvent constructor.
     *
     * @param string event
     * @since 9.1.0
     */
    public SystemTagsEntityEvent(string @event)
    {
        this.@event = @event;
        this.collections = new Dictionary<Action>();
    }

    /**
     * @param string name
     * @param \Closure entityExistsFunction The closure should take one
     *                 argument, which is the id of the entity, that tags
     *                 should be handled for. The return should then be bool,
     *                 depending on whether tags are allowed (true) or not.
     * @throws \OutOfBoundsException when the entity name is already taken
     * @since 9.1.0
     */
    public void addEntityCollection(string name, Action entityExistsFunction)
    {
            if(this.collections.ContainsKey(name))
            {
                throw new OutOfBoundsException(@"Duplicate entity name ${name}. ");
            }

        this.collections[name] = entityExistsFunction;
    }

    /**
     * @return \Closure[]
     * @since 9.1.0
     */
    public IList<Action> getEntityCollections() {
        return this.collections.Values;
    }
}

}
