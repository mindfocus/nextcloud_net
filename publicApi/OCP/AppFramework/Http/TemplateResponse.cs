using System.Collections.Generic;

namespace OCP.AppFramework.Http
{
/**
 * Response for a normal template
 * @since 6.0.0
 */
public class TemplateResponse : Response {

	const string EVENT_LOAD_ADDITIONAL_SCRIPTS = self::class . '::loadAdditionalScripts';
	const string EVENT_LOAD_ADDITIONAL_SCRIPTS_LOGGEDIN = self::class . '::loadAdditionalScriptsLoggedIn';

	/**
	 * name of the template
	 * @var string
	 */
	protected string templateName;

	/**
	 * parameters
	 * @var array
	 */
	protected IDictionary<string,object>  paramters;

	/**
	 * rendering type (admin, user, blank)
	 * @var string
	 */
	protected string _renderAs;

	/**
	 * app name
	 * @var string
	 */
	protected string appName;

	/**
	 * constructor of TemplateResponse
	 * @param string appName the name of the app to load the template from
	 * @param string templateName the name of the template
	 * @param array params an array of parameters which should be passed to the
	 * template
	 * @param string renderAs how the page should be rendered, defaults to user
	 * @since 6.0.0 - parameters params and renderAs were added in 7.0.0
	 */
	public TemplateResponse(string appName, string templateName, IDictionary<string,object> paramters,
	                            string renderAs="user") {
		this.templateName = templateName;
		this.appName = appName;
		this.paramters = paramters;
		this._renderAs = renderAs;
	}


	/**
	 * Sets template parameters
	 * @param array params an array with key => value structure which sets template
	 *                      variables
	 * @return TemplateResponse Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public TemplateResponse setParams(IDictionary<string,object> paramters){
		this.paramters = paramters;

		return this;
	}


	/**
	 * Used for accessing the set parameters
	 * @return array the params
	 * @since 6.0.0
	 */
	public IDictionary<string,object> getParams(){
		return this.paramters;
	}


	/**
	 * Used for accessing the name of the set template
	 * @return string the name of the used template
	 * @since 6.0.0
	 */
	public string getTemplateName(){
		return this.templateName;
	}


	/**
	 * Sets the template page
	 * @param string renderAs admin, user or blank. Admin also prints the admin
	 *                         settings header and footer, user renders the normal
	 *                         normal page including footer and header and blank
	 *                         just renders the plain template
	 * @return TemplateResponse Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public TemplateResponse renderAs(string renderAs){
		this._renderAs = renderAs;

		return this;
	}


	/**
	 * Returns the set renderAs
	 * @return string the renderAs value
	 * @since 6.0.0
	 */
	public string getRenderAs(){
		return this._renderAs;
	}


	/**
	 * Returns the rendered html
	 * @return string the rendered html
	 * @since 6.0.0
	 */
	public string render(){
		// \OCP\Template needs an empty string instead of 'blank' for an unwrapped response
		var renderAs = this._renderAs == "blank" ? "" : this._renderAs;

		var template = new OCP.Template(this.appName, this.templateName, renderAs);

		foreach(var param in this.paramters){
			template.assign(param.Key, param.Value);
		}

		return template.fetchPage(this.paramters);
	}

}

}