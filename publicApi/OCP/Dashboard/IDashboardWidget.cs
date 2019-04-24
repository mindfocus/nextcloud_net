using OCP.Dashboard.Model;

namespace OCP.Dashboard
{
/**
 * Interface IDashboardWidget
 *
 * This interface is used to create a widget: the widget must implement this
 * interface and be defined in appinfo/info.xml:
 *
 *    <dashboard>
 *      <widget>OCA\YourApp\YourWidget</widget>
 *  </dashboard>
 *
 * Multiple widget can be defined in the same appinfo/info.xml.
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard
 */
public interface IDashboardWidget {

	/**
	 * Should returns the (unique) Id of the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	string getId();


	/**
	 * Should returns the [display] name of the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	string getName();


	/**
	 * Should returns some text describing the widget.
	 * This description is displayed in the listing of the available widgets.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	string getDescription();


	/**
	 * Must generate and return a WidgetTemplate that define important stuff
	 * about the Widget: icon, content, css or javascript.
	 *
	 * @see WidgetTemplate
	 *
	 * @since 15.0.0
	 *
	 * @return WidgetTemplate
	 */
	WidgetTemplate getWidgetTemplate();


	/**
	 * Must create and return a WidgetSetup containing the general setup of
	 * the widget
	 *
	 * @see WidgetSetup
	 *
	 * @since 15.0.0
	 *
	 * @return WidgetSetup
	 */
	WidgetSetup getWidgetSetup();


	/**
	 * This method is called when a widget is loaded on the dashboard.
	 * A widget is 'loaded on the dashboard' when one of these conditions
	 * occurs:
	 *
	 * - the user is adding the widget on his dashboard,
	 * - the user already added the widget on his dashboard and he is opening
	 *   the dashboard app.
	 *
	 * @see IWidgetConfig
	 *
	 * @since 15.0.0
	 *
	 * @param IWidgetConfig settings
	 */
	void loadWidget(IWidgetConfig settings);


	/**
	 * This method s executed when the widget call the net.requestWidget()
	 * from the Javascript API.
	 *
	 * This is used by the frontend to communicate with the backend.
	 *
	 * @see IWidgetRequest
	 *
	 * @since 15.0.0
	 *
	 * @param IWidgetRequest request
	 */
	void requestWidget(IWidgetRequest request);

}
}