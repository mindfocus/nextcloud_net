using OCP.Dashboard.Model;

namespace OCP.Dashboard.Service
{
/**
 * Interface IWidgetsService
 *
 * The Service is provided by the Dashboard app. The method in this interface
 * are used by the IDashboardManager when a widget needs to access the current
 * configuration of a widget for a user.
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Service
 */
    public interface IWidgetsService {

        /**
         * Returns the IWidgetConfig for a widgetId and userId
         *
         * @since 15.0.0
         *
         * @param string widgetId
         * @param string userId
         *
         * @return IWidgetConfig
         */
        IWidgetConfig getWidgetConfig(string widgetId, string userId);

    }


}