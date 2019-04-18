using System;
using System.Collections.Generic;
using System.Text;
using publicApi.ext;

namespace OCP.App
{
    /**
     * Class ManagerEvent
     *
     * @package OCP\APP
     * @since 9.0.0
     */
    class ManagerEvent : Event
    {

	// string EVENT_APP_ENABLE = @"OCP\App\IAppManager::enableApp";
	// const EVENT_APP_ENABLE_FOR_GROUPS = 'OCP\App\IAppManager::enableAppForGroups';
	// const EVENT_APP_DISABLE = 'OCP\App\IAppManager::disableApp';

	/**
	 * @since 9.1.0
	 */
	// const EVENT_APP_UPDATE = 'OCP\App\IAppManager::updateApp';

    /** @var string */
    protected string @event;
    /** @var string */
    protected string appID;
	/** @var \OCP\IGroup[]|null */
	protected IList<IGroup> groups;

	/**
	 * DispatcherEvent constructor.
	 *
	 * @param string $event
	 * @param $appID
	 * @param \OCP\IGroup[]|null $groups
	 * @since 9.0.0
	 */
	public ManagerEvent(string @event, string appID, IList<IGroup> groups = null)
    {
        this.@event = @event;
        this.appID = appID;
        this.groups = groups;
    }

    /**
	 * @return string
	 * @since 9.0.0
	 */
    public string getEvent()
    {
        return this.@event;
    }

    /**
	 * @return string
	 * @since 9.0.0
	 */
    public string getAppID()
    {
        return this.appID;
    }

    /**
	 * returns the group Ids
	 * @return string[]
	 * @since 9.0.0
	 */
    public IList<string> getGroups()
    {
        var result = new List<string>();
        foreach (var item in this.groups)
        {
            result.Add(item.getGID());
        }
        return result;
    }
}

}
