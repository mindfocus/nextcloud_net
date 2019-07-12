using System.Collections;
using System.Collections.Generic;
using OCP.Dashboard.Model;
using OCP.Dashboard.Service;

namespace OCP.Dashboard
{
/**
 * Interface IDashboardManager
 *use OCP\Dashboard\Exceptions\DashboardAppNotAvailableException;
 *use OCP\Dashboard\Model\IWidgetConfig;
*use OCP\Dashboard\Service\IEventsService;
*use OCP\Dashboard\Service\IWidgetsService;
 * IDashboardManager should be used to manage widget from the backend.
 * The call can be done from any Service.
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard
 */
interface IDashboardManager {


	/**
	 * Register a IWidgetsService.
	 *
	 * @since 15.0.0
	 *
	 * @param IWidgetsService widgetsService
	 */
	void registerWidgetsService(IWidgetsService widgetsService);


	/**
	 * Register a IEventsService.
	 *
	 * @since 15.0.0
	 *
	 * @param IEventsService eventsService
	 */
	void registerEventsService(IEventsService eventsService);


	/**
	 * returns the OCP\Dashboard\Model\IWidgetConfig for a widgetId and userId.
	 *
	 * @see IWidgetConfig
	 *
	 * @since 15.0.0
	 *
	 * @param string widgetId
	 * @param string userId
	 *
	 * @throws DashboardAppNotAvailableException
	 * @return IWidgetConfig
	 */
	IWidgetConfig getWidgetConfig(string widgetId, string userId);


	/**
	 * Create push notifications for users.
	 * payload is an array that will be send to the Javascript method
	 * called on push.
	 * uniqueId needs to be used if you send the push to multiples users
	 * and multiples groups so that one user does not have duplicate
	 * notifications.
	 *
	 * Push notifications are created in database and broadcast to user
	 * that are running dashboard.
	 *
	 * @since 15.0.0
	 *
	 * @param string widgetId
	 * @param array users
	 * @param array payload
	 * @param string uniqueId
	 * @throws DashboardAppNotAvailableException
	 */
	void createUsersEvent(string widgetId,  IList<string> users,  IList<string> payload, string uniqueId = "");


	/**
	 * Create push notifications for groups. (ie. createUsersEvent())
	 *
	 * @since 15.0.0
	 *
	 * @param string widgetId
	 * @param array groups
	 * @param array payload
	 * @param string uniqueId
	 * @throws DashboardAppNotAvailableException
	 */
	void createGroupsEvent(string widgetId, IList<string> groups, IList<string> payload, string uniqueId = "");


	/**
	 * Create push notifications for everyone. (ie. createUsersEvent())
	 *
	 * @since 15.0.0
	 *
	 * @param string widgetId
	 * @param array payload
	 * @param string uniqueId
	 * @throws DashboardAppNotAvailableException
	 */
	void createGlobalEvent(string widgetId, IList<string> payload, string uniqueId = "");

}

}