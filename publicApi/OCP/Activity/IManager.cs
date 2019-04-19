using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Activity
{
    /**
     * Interface IManager
     *
     * @package OCP\Activity
     * @since 6.0.0
     */
    public interface IManager
    {
        /**
         * Generates a new IEvent object
         *
         * Make sure to call at least the following methods before sending it to the
         * app with via the publish() method:
         *  - setApp()
         *  - setType()
         *  - setAffectedUser()
         *  - setSubject()
         *
         * @return IEvent
         * @since 8.2.0
         */
        IEvent generateEvent();

        /**
         * Publish an event to the activity consumers
         *
         * Make sure to call at least the following methods before sending an Event:
         *  - setApp()
         *  - setType()
         *  - setAffectedUser()
         *  - setSubject()
         *
         * @param IEvent $event
         * @throws \BadMethodCallException if required values have not been set
         * @since 8.2.0
         */
        void publish(IEvent pEvent);

	/**
	 * In order to improve lazy loading a closure can be registered which will be called in case
	 * activity consumers are actually requested
	 *
	 * $callable has to return an instance of \OCP\Activity\IConsumer
	 *
	 * @param \Closure $callable
	 * @since 6.0.0
	 */
	void registerConsumer(Action callable);

	/**
	 * @param string $filter Class must implement OCA\Activity\IFilter
	 * @since 11.0.0
	 */
	void registerFilter(string filter);

	/**
	 * @return IFilter[]
	 * @since 11.0.0
	 */
	IList<IFilter> getFilters();

	/**
	 * @param string $id
	 * @return IFilter
	 * @throws \InvalidArgumentException when the filter was not found
	 * @since 11.0.0
	 */
	IFilter getFilterById(string id);

	/**
	 * @param string $setting Class must implement OCA\Activity\ISetting
	 * @since 11.0.0
	 */
	void registerSetting(string setting);

	/**
	 * @return ISetting[]
	 * @since 11.0.0
	 */
	IList<ISetting> getSettings();

	/**
	 * @param string $provider Class must implement OCA\Activity\IProvider
	 * @since 11.0.0
	 */
	void registerProvider(string provider);

	/**
	 * @return IProvider[]
	 * @since 11.0.0
	 */
	IList<IProvider> getProviders();

	/**
	 * @param string $id
	 * @return ISetting
	 * @throws \InvalidArgumentException when the setting was not found
	 * @since 11.0.0
	 */
	ISetting getSettingById(string id);

	/**
	 * @param string $type
	 * @param int $id
	 * @since 8.2.0
	 */
	void setFormattingObject(string type, int id);

	/**
	 * @return bool
	 * @since 8.2.0
	 */
	bool isFormattingFilteredObject();

	/**
	 * @param bool $status Set to true, when parsing events should not use SVG icons
	 * @since 12.0.1
	 */
	void setRequirePNG(bool status);

	/**
	 * @return bool
	 * @since 12.0.1
	 */
	bool getRequirePNG();

	/**
	 * Set the user we need to use
	 *
	 * @param string|null $currentUserId
	 * @throws \UnexpectedValueException If the user is invalid
	 * @since 9.0.1
	 */
	void setCurrentUserId(string currentUserId = null);

	/**
	 * Get the user we need to use
	 *
	 * Either the user is logged in, or we try to get it from the token
	 *
	 * @return string
	 * @throws \UnexpectedValueException If the token is invalid, does not exist or is not unique
	 * @since 8.1.0
	 */
	string getCurrentUserId();
}

}
