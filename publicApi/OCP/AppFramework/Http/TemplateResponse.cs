namespace OCP.AppFramework.Http
{
/**
 * Response for a normal template
 * @since 6.0.0
 */
public class TemplateResponse : Response {

	const EVENT_LOAD_ADDITIONAL_SCRIPTS = self::class . '::loadAdditionalScripts';
	const EVENT_LOAD_ADDITIONAL_SCRIPTS_LOGGEDIN = self::class . '::loadAdditionalScriptsLoggedIn';

	/**
	 * name of the template
	 * @var string
	 */
	protected templateName;

	/**
	 * parameters
	 * @var array
	 */
	protected params;

	/**
	 * rendering type (admin, user, blank)
	 * @var string
	 */
	protected renderAs;

	/**
	 * app name
	 * @var string
	 */
	protected appName;

	/**
	 * constructor of TemplateResponse
	 * @param string appName the name of the app to load the template from
	 * @param string templateName the name of the template
	 * @param array params an array of parameters which should be passed to the
	 * template
	 * @param string renderAs how the page should be rendered, defaults to user
	 * @since 6.0.0 - parameters params and renderAs were added in 7.0.0
	 */
	public function __construct(appName, templateName, array params=array(),
	                            renderAs='user') {
		this->templateName = templateName;
		this->appName = appName;
		this->params = params;
		this->renderAs = renderAs;
	}


	/**
	 * Sets template parameters
	 * @param array params an array with key => value structure which sets template
	 *                      variables
	 * @return TemplateResponse Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public function setParams(array params){
		this->params = params;

		return this;
	}


	/**
	 * Used for accessing the set parameters
	 * @return array the params
	 * @since 6.0.0
	 */
	public function getParams(){
		return this->params;
	}


	/**
	 * Used for accessing the name of the set template
	 * @return string the name of the used template
	 * @since 6.0.0
	 */
	public function getTemplateName(){
		return this->templateName;
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
	public function renderAs(renderAs){
		this->renderAs = renderAs;

		return this;
	}


	/**
	 * Returns the set renderAs
	 * @return string the renderAs value
	 * @since 6.0.0
	 */
	public function getRenderAs(){
		return this->renderAs;
	}


	/**
	 * Returns the rendered html
	 * @return string the rendered html
	 * @since 6.0.0
	 */
	public function render(){
		// \OCP\Template needs an empty string instead of 'blank' for an unwrapped response
		renderAs = this->renderAs === 'blank' ? '' : this->renderAs;

		template = new \OCP\Template(this->appName, this->templateName, renderAs);

		foreach(this->params as key => value){
			template->assign(key, value);
		}

		return template->fetchPage(this->params);
	}

}

}