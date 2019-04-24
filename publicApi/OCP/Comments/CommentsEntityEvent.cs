using System;
using System.Collections;
using System.Collections.Generic;

namespace OCP.Comments
{
/**
 * Class CommentsEntityEvent
 *
 * @package OCP\Comments
 * @since 9.1.0
 */
    public class CommentsEntityEvent : ext.Event {

    const string EVENT_ENTITY = @"OCP\Comments\ICommentsManager::registerEntity";

    /** @var string */
    protected string @event;
    /** @var \Closure[] */
    protected IDictionary<string,Action> collections;

    /**
     * DispatcherEvent constructor.
     *
     * @param string event
     * @since 9.1.0
     */
    public CommentsEntityEvent(string @event)
    {
        this.@event = @event;
        this.collections = new Dictionary<string, Action>();
    }

    /**
     * @param string name
     * @param \Closure entityExistsFunction The closure should take one
     *                 argument, which is the id of the entity, that comments
     *                 should be handled for. The return should then be bool,
     *                 depending on whether comments are allowed (true) or not.
     * @throws \OutOfBoundsException when the entity name is already taken
     * @since 9.1.0
     */
    public void addEntityCollection(string name, Action entityExistsFunction) {
        if (!this.collections.ContainsKey(name)) {
            throw new OutOfBoundsException("Duplicate entity name " + name );
        }
        this.collections[name] = entityExistsFunction;
    }

    /**
     * @return \Closure[]
     * @since 9.1.0
     */
    public IDictionary<string,Action> getEntityCollections() {
        return this.collections;
    }
    }
}