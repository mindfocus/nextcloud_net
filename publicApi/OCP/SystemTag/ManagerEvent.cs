using System;
namespace OCP.SystemTag
{
    /**
     * Class ManagerEvent
     *
     * @package OCP\SystemTag
     * @since 9.0.0
     */
    class ManagerEvent : ext.Event
    {

    const string EVENT_CREATE = @"OCP\\SystemTag\\ISystemTagManager::createTag";
    const string EVENT_UPDATE = @"OCP\\SystemTag\\ISystemTagManager::updateTag";
    const string EVENT_DELETE = @"OCP\\SystemTag\\ISystemTagManager::deleteTag";

    /** @var string */
    protected string @event;
    /** @var ISystemTag */
    protected ISystemTag tag;
    /** @var ISystemTag */
    protected ISystemTag beforeTag;

    /**
     * DispatcherEvent constructor.
     *
     * @param string event
     * @param ISystemTag tag
     * @param ISystemTag|null beforeTag
     * @since 9.0.0
     */
    public ManagerEvent(string @event, ISystemTag tag, ISystemTag beforeTag = null)
    {
        this.@event = @event;
        this.tag = tag;
        this.beforeTag = beforeTag;
    }

    /**
     * @return string
     * @since 9.0.0
     */
    public string getEvent() {
        return this.@event;
    }

    /**
     * @return ISystemTag
     * @since 9.0.0
     */
    public ISystemTag getTag() {
        return this.tag;
    }

    /**
     * @return ISystemTag
     * @since 9.0.0
     * @throws \BadMethodCallException
     */
    public ISystemTag getTagBefore()  {
        if (this.@event != EVENT_UPDATE) {
        throw new BadMethodCallException("getTagBefore is only available on the update Event");
    }
        return this.beforeTag;
    }
}

}
