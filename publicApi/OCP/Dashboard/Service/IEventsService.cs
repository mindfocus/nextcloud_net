using System.Collections.Generic;

namespace OCP.Dashboard.Service
{
/**
 * Interface IEventsService
 *
 * The Service is provided by the Dashboard app. The method in this interface
 * are used by the IDashboardManager when creating push event.
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Service
 */
    public interface IEventsService {


        /**
         * Create an event for a widget and an array of users.
         *
         * @see IDashboardManager::createUsersEvent
         *
         * @since 15.0.0
         *
         * @param string widgetId
         * @param array users
         * @param array payload
         * @param string uniqueId
         */
        void createUsersEvent(string widgetId, IList<string> users, IList<string> payload, string uniqueId);


        /**
         * Create an event for a widget and an array of groups.
         *
         * @see IDashboardManager::createGroupsEvent
         *
         * @since 15.0.0
         *
         * @param string widgetId
         * @param array groups
         * @param array payload
         * @param string uniqueId
         */
        void createGroupsEvent(string widgetId, IList<string> groups, IList<string> payload, string uniqueId);


        /**
         * Create a global event for all users that use a specific widget.
         *
         * @see IDashboardManager::createGlobalEvent
         *
         * @since 15.0.0
         *
         * @param string widgetId
         * @param array payload
         * @param string uniqueId
         */
        void createGlobalEvent(string widgetId, IList<string> payload, string uniqueId);


    }
}