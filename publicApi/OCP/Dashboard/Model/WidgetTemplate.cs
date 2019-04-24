using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
/**
 * Interface WidgetTemplate
 *
 * A widget must create an WidgetTemplate object and returns it in the
 * IDashboardWidget::getWidgetTemplate method.
 *
 * @see IDashboardWidget::getWidgetTemplate
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Model
 */
public sealed class WidgetTemplate {


	/** @var string */
	private string icon = "";

	/** @var array */
	private IList<string> css = new List<string>();

	/** @var array */
	private IList<string> js = new List<string>();

	/** @var string */
	private string content = "";

	/** @var string */
	private string function = "";

	/** @var WidgetSetting[] */
	private IDictionary<string ,WidgetSetting> settings = new Dictionary<string, WidgetSetting>();


	/**
	 * Get the icon class of the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getIcon(){
		return this.icon;
	}

	/**
	 * Set the icon class of the widget.
	 * This class must be defined in one of the CSS file used by the widget.
	 *
	 * @see addCss
	 *
	 * @since 15.0.0
	 *
	 * @param string icon
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setIcon(string icon){
		this.icon = icon;
		return this;
	}

	/**
	 * Get CSS files to be included when displaying a widget
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IList<string> getCss(){
		return this.css;
	}

	/**
	 * path and name of CSS files
	 *
	 * @since 15.0.0
	 *
	 * @param array css
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setCss(IList<string> css)  {
		this.css = css;

		return this;
	}

	/**
	 * Add a CSS file to be included when displaying a widget.
	 *
	 * @since 15.0.0
	 *
	 * @param string css
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate addCss(string css) {
		this.css.Add(css);

		return this;
	}

	/**
	 * Get JS files to be included when loading a widget
	 *
	 * @since 15.0.0
	 *
	 * @return array
	 */
	public IList<string> getJs() {
		return this.js;
	}

	/**
	 * Set an array of JS files to be included when loading a widget.
	 *
	 * @since 15.0.0
	 *
	 * @param array js
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setJs(IList<string> js) {
		this.js = js;

		return this;
	}

	/**
	 * Add a JS file to be included when loading a widget.
	 *
	 * @since 15.0.0
	 *
	 * @param string js
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate addJs(string js)  {
		this.js.Add(js);

		return this;
	}

	/**
	 * Get the HTML file that contains the content of the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getContent() {
		return this.content;
	}

	/**
	 * Set the HTML file that contains the content of the widget.
	 *
	 * @since 15.0.0
	 *
	 * @param string content
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setContent(string content) {
		this.content = content;

		return this;
	}

	/**
	 * Get the JS function to be called when loading the widget.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getInitFunction(){
		return this.function;
	}

	/**
	 * JavaScript function to be called when loading the widget on the
	 * dashboard
	 *
	 * @since 15.0.0
	 *
	 * @param string function
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setInitFunction(string function){
		this.function = function;

		return this;
	}

	/**
	 * Get all WidgetSetting defined for the widget.
	 *
	 * @see WidgetSetting
	 *
	 * @since 15.0.0
	 *
	 * @return WidgetSetting[]
	 */
	public IDictionary<string ,WidgetSetting> getSettings(){
		return this.settings;
	}

	/**
	 * Define all WidgetSetting for the widget.
	 *
	 * @since 15.0.0
	 *
	 * @see WidgetSetting
	 *
	 * @param WidgetSetting[] settings
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate setSettings(IDictionary<string ,WidgetSetting> settings) {
		this.settings = settings;

		return this;
	}

	/**
	 * Add a WidgetSetting.
	 *
	 * @see WidgetSetting
	 *
	 * @since 15.0.0
	 *
	 * @param WidgetSetting setting
	 *
	 * @return WidgetTemplate
	 */
	public WidgetTemplate addSetting(WidgetSetting setting)  {
		this.settings.Add(setting.getName(), setting);

		return this;
	}

	/**
	 * Get a WidgetSetting by its name
	 *
	 * @see WidgetSetting::setName
	 *
	 * @since 15.0.0
	 *
	 * @param string key
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting getSetting(string key){
        if (this.settings.ContainsKey(key))
        {
            return this.settings[key];
        }

        return null;
	}


        /**
        // * @since 15.0.0
        // *
        // * @return array
         */
        //public function jsonSerialize() {
        //	return [
        //		'icon' => this.getIcon(),
        //		'css' => this.getCss(),
        //		'js' => this.getJs(),
        //		'content' => this.getContent(),
        //		'function' => this.getInitFunction(),
        //		'settings' => this.getSettings()
        //	];
        //}


    }


}
