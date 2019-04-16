using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.App
{
    /**
     * Class ManagerEvent
     *
     * @package OCP\APP
     * @since 9.0.0
     */
    class ManagerEvent : Event
    {

	string EVENT_APP_ENABLE = @"OCP\App\IAppManager::enableApp";
	const EVENT_APP_ENABLE_FOR_GROUPS = 'OCP\App\IAppManager::enableAppForGroups';
	const EVENT_APP_DISABLE = 'OCP\App\IAppManager::disableApp';

	/**
	 * @since 9.1.0
	 */
	const EVENT_APP_UPDATE = 'OCP\App\IAppManager::updateApp';

    /** @var string */
    protected $event;
    /** @var string */
    protected $appID;
	/** @var \OCP\IGroup[]|null */
	protected $groups;

	/**
	 * DispatcherEvent constructor.
	 *
	 * @param string $event
	 * @param $appID
	 * @param \OCP\IGroup[]|null $groups
	 * @since 9.0.0
	 */
	public function __construct($event, $appID, array $groups = null)
    {
		$this->event = $event;
		$this->appID = $appID;
		$this->groups = $groups;
    }

    /**
	 * @return string
	 * @since 9.0.0
	 */
    public function getEvent()
    {
        return $this->event;
    }

    /**
	 * @return string
	 * @since 9.0.0
	 */
    public function getAppID()
    {
        return $this->appID;
    }

    /**
	 * returns the group Ids
	 * @return string[]
	 * @since 9.0.0
	 */
    public function getGroups()
    {
        return array_map(function($group) {
            /** @var \OCP\IGroup $group */
            return $group->getGID();
        }, $this->groups);
    }
}

}
