using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
/**
 * Interface IWidgetRequest
 *
 * WidgetRequest are created by the Dashboard App and used to communicate from
 * the frontend to the backend.
 * The object is send to the WidgetClass using IDashboardWidget::requestWidget
 *
 * @see IDashboardWidget::requestWidget
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Model
 */
    public interface IWidgetRequest {

        /**
         * Get the widgetId.
         *
         * @since 15.0.0
         *
         * @return string
         */
        string getWidgetId();


        /**
         * Get the WidgetClass.
         *
         * @since 15.0.0
         *
         * @return IDashboardWidget
         */
        IDashboardWidget getWidget();


        /**
         * Get the 'request' string sent by the request from the front-end with
         * the format:
         *
         *  net.requestWidget(
         *    {
         *     widget: widgetId,
         *     request: request,
         *     value: value
         *    },
         *    callback);
         *
         * @since 15.0.0
         *
         * @return string
         */
        string getRequest();


        /**
         * Get the 'value' string sent by the request from the front-end.
         *
         * @see getRequest
         *
         * @since 15.0.0
         *
         * @return string
         */
        string getValue();


        /**
         * Returns the result.
         *
         * @since 15.0.0
         *
         * @return array
         */
        IList<string> getResult();


        /**
         * add a result (as string)
         *
         * @since 15.0.0
         *
         * @param string key
         * @param string result
         *
         * @return this
         */
        IWidgetRequest addResult(string key, string result);

        /**
         * add a result (as array)
         *
         * @since 15.0.0
         *
         * @param string key
         * @param array result
         *
         * @return this
         */
        IWidgetRequest addResultArray(string key, IList<string> result);

    }


}
