using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
/**
 * Interface WidgetSetup
 *
 * A widget must create an WidgetSetup object and returns it in the
 * IDashboardWidget::getWidgetSetup method.
 *
 * @see IDashboardWidget::getWidgetSetup
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Model
 */
public sealed class WidgetSetup {


	const string SIZE_TYPE_MIN = "min";
	const string SIZE_TYPE_MAX = "max";
	const string SIZE_TYPE_DEFAULT = "default";


	/** @var array */
	private IDictionary<string, IDictionary<string, int>> sizes = new Dictionary<string, IDictionary<string, int>>();

	/** @var array */
	private IList<WidgetMenuEntry> menus = new List<WidgetMenuEntry>();

	/** @var array */
	private IList<WidgetDelayedJob> jobs = new List<WidgetDelayedJob>();

	/** @var string */
	private string push = "";

	/** @var array */
	private IList<string> settings = new List<string>();


	/**
	 * Get the defined size for a specific type (min, max, default)
	 * Returns an array:
	 * [
	 *   'width' => width,
	 *   'height' => height
	 * ]
	 *
	 *
	 * @since 15.0.0
	 *
	 * @param string type
	 *
	 * @return array
	 */
	public IDictionary<string, int> getSize(string type) {
        if (this.sizes.ContainsKey(type))
        {
            return this.sizes[type];
        }
		return new Dictionary<string, int>();
	}

	/**
	 * Returns all sizes defined for the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IDictionary<string, IDictionary<string, int>> getSizes() {
		return this.sizes;
	}

	/**
	 * Add a new size to the setup.
	 *
	 * @since 15.0.0
	 *
	 * @param string type
	 * @param int width
	 * @param int height
	 *
	 * @return WidgetSetup
	 */
	public WidgetSetup addSize(string type, int width, int height) {
        this.sizes[type] = new Dictionary<string, int>();
        this.sizes[type]["width"] = width;
        this.sizes[type]["height"] = height;
		return this;
	}

	/**
	 * Returns menu entries.
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IList<WidgetMenuEntry> getMenuEntries(){
		return this.menus;
	}

	/**
	 * Add a menu entry to the widget.
	 * function is the Javascript function to be called when clicking the
	 *           menu entry.
	 * icon is the css class of the icon.
	 * text is the display name of the menu entry.
	 *
	 * @since 15.0.0
	 *
	 * @param string function
	 * @param string icon
	 * @param string text
	 *
	 * @return WidgetSetup
	 */
	public WidgetSetup addMenuEntry(string function, string icon, string text) {
		this.menus.Add(new WidgetMenuEntry(function,icon,text));
		return this;
	}


	/**
	 * Add a delayed job to the widget.
	 *
	 * function is the Javascript function to be called.
	 * delay is the time in seconds between each call.
	 *
	 * @since 15.0.0
	 *
	 * @param string function
	 * @param int delay
	 *
	 * @return WidgetSetup
	 */
	public WidgetSetup addDelayedJob(string function, int delay) {
		this.jobs.Add(new WidgetDelayedJob(function,delay)); 
		return this;
	}

	/**
	 * Get delayed jobs.
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IList<WidgetDelayedJob> getDelayedJobs() {
		return this.jobs;
	}


	/**
	 * Get the push function, called when an event is send to the front-end
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getPush() {
		return this.push;
	}

	/**
	 * Set the Javascript function to be called when an event is pushed to the
	 * frontend.
	 *
	 * @since 15.0.0
	 *
	 * @param string function
	 *
	 * @return WidgetSetup
	 */
	public WidgetSetup setPush(string function) {
		this.push = function;
		return this;
	}


	/**
	 * Returns the default settings for a widget.
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IList<string> getDefaultSettings(){
		return this.settings;
	}

	/**
	 * Set the default settings for a widget.
	 * This method is used by the Dashboard app, using the settings created
	 * using WidgetSetting
	 *
	 * @see WidgetSetting
	 *
	 * @since 15.0.0
	 *
	 * @param array settings
	 *
	 * @return WidgetSetup
	 */
	public WidgetSetup setDefaultSettings(IList<string> settings) {
		this.settings = settings;
		return this;
	}


    /**
	// * @since 15.0.0
	// *
	// * @return array
	 */
    //public function jsonSerialize() {
    //	return [
    //		'size' => this.getSizes(),
    //		'menu' => this.getMenuEntries(),
    //		'jobs' => this.getDelayedJobs(),
    //		'push' => this.getPush(),
    //		'settings' => this.getDefaultSettings()
    //	];
    //}
}


}
