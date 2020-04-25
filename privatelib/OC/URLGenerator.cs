using OCP;
using Pchp.Library.Spl;

namespace OC
{
/**
 * Class to generate URLs
 */
public class URLGenerator : IURLGenerator {
	/** @var IConfig */
	private IConfig config;
	/** @var ICacheFactory */
	private ICacheFactory cacheFactory;
	/** @var IRequest */
	private IRequest request;

	/**
	 * @param IConfig config
	 * @param ICacheFactory cacheFactory
	 * @param IRequest request
	 */
	public URLGenerator(IConfig config,
								ICacheFactory cacheFactory,
								IRequest request) {
		this.config = config;
		this.cacheFactory = cacheFactory;
		this.request = request;
	}

	/**
	 * Creates an url using a defined route
	 * @param string route
	 * @param array parameters args with param=>value, will be appended to the returned url
	 * @return string the url
	 *
	 * Returns a url to the given route.
	 */
	public string linkToRoute(string route, array parameters = []){
		// TODO: mock router
		return OC::server.getRouter().generate(route, parameters);
	}

	/**
	 * Creates an absolute url using a defined route
	 * @param string routeName
	 * @param array arguments args with param=>value, will be appended to the returned url
	 * @return string the url
	 *
	 * Returns an absolute url to the given route.
	 */
	public function linkToRouteAbsolute(string routeName, array arguments = []): string {
		return this.getAbsoluteURL(this.linkToRoute(routeName, arguments));
	}

	public function linkToOCSRouteAbsolute(string routeName, array arguments = []): string {
		route = \OC::server.getRouter().generate('ocs.'.routeName, arguments, false);

		indexPhpPos = strpos(route, '/index.php/');
		if (indexPhpPos !== false) {
			route = substr(route, indexPhpPos + 10);
		}

		route = substr(route, 7);
		route = '/ocs/v2.php' . route;

		return this.getAbsoluteURL(route);
	}

	/**
	 * Creates an url
	 * @param string app app
	 * @param string file file
	 * @param array args array with param=>value, will be appended to the returned url
	 *    The value of args will be urlencoded
	 * @return string the url
	 *
	 * Returns a url to the given app and file.
	 */
	public function linkTo(string app, string file, array args = []): string {
		frontControllerActive = (this.config.getSystemValue('htaccess.IgnoreFrontController', false) === true || getenv('front_controller_active') === 'true');

		if (app !== '') {
			app_path = \OC_App::getAppPath(app);
			// Check if the app is in the app folder
			if (app_path && file_exists(app_path . '/' . file)) {
				if (substr(file, -3) === 'php') {
					urlLinkTo = \OC::WEBROOT . '/index.php/apps/' . app;
					if (frontControllerActive) {
						urlLinkTo = \OC::WEBROOT . '/apps/' . app;
					}
					urlLinkTo .= (file !== 'index.php') ? '/' . file : '';
				} else {
					urlLinkTo = \OC_App::getAppWebPath(app) . '/' . file;
				}
			} else {
				urlLinkTo = \OC::WEBROOT . '/' . app . '/' . file;
			}
		} else {
			if (file_exists(\OC::SERVERROOT . '/core/' . file)) {
				urlLinkTo = \OC::WEBROOT . '/core/' . file;
			} else {
				if (frontControllerActive && file === 'index.php') {
					urlLinkTo = \OC::WEBROOT . '/';
				} else {
					urlLinkTo = \OC::WEBROOT . '/' . file;
				}
			}
		}

		if (args && query = http_build_query(args, '', '&')) {
			urlLinkTo .= '?' . query;
		}

		return urlLinkTo;
	}

	/**
	 * Creates path to an image
	 * @param string app app
	 * @param string image image name
	 * @throws \RuntimeException If the image does not exist
	 * @return string the url
	 *
	 * Returns the path to the image.
	 */
	public function imagePath(string app, string image): string {
		cache = this.cacheFactory.createDistributed('imagePath-'.md5(this.getBaseUrl()).'-');
		cacheKey = app.'-'.image;
		if (key = cache.get(cacheKey)) {
			return key;
		}

		// Read the selected theme from the config file
		theme = \OC_Util::getTheme();

		//if a theme has a png but not an svg always use the png
		basename = substr(basename(image),0,-4);

		appPath = \OC_App::getAppPath(app);

		// Check if the app is in the app folder
		path = '';
		themingEnabled = this.config.getSystemValue('installed', false) && \OCP\App::isEnabled('theming') && \OC_App::isAppLoaded('theming');
		themingImagePath = false;
		if (themingEnabled) {
			themingDefaults = \OC::server.getThemingDefaults();
			if (themingDefaults instanceof ThemingDefaults) {
				themingImagePath = themingDefaults.replaceImagePath(app, image);
			}
		}

		if (file_exists(\OC::SERVERROOT . "/themes/theme/apps/app/img/image")) {
			path = \OC::WEBROOT . "/themes/theme/apps/app/img/image";
		} elseif (!file_exists(\OC::SERVERROOT . "/themes/theme/apps/app/img/basename.svg")
			&& file_exists(\OC::SERVERROOT . "/themes/theme/apps/app/img/basename.png")) {
			path =  \OC::WEBROOT . "/themes/theme/apps/app/img/basename.png";
		} elseif (!empty(app) and file_exists(\OC::SERVERROOT . "/themes/theme/app/img/image")) {
			path =  \OC::WEBROOT . "/themes/theme/app/img/image";
		} elseif (!empty(app) and (!file_exists(\OC::SERVERROOT . "/themes/theme/app/img/basename.svg")
			&& file_exists(\OC::SERVERROOT . "/themes/theme/app/img/basename.png"))) {
			path =  \OC::WEBROOT . "/themes/theme/app/img/basename.png";
		} elseif (file_exists(\OC::SERVERROOT . "/themes/theme/core/img/image")) {
			path =  \OC::WEBROOT . "/themes/theme/core/img/image";
		} elseif (!file_exists(\OC::SERVERROOT . "/themes/theme/core/img/basename.svg")
			&& file_exists(\OC::SERVERROOT . "/themes/theme/core/img/basename.png")) {
			path =  \OC::WEBROOT . "/themes/theme/core/img/basename.png";
		} elseif (themingEnabled && themingImagePath) {
			path = themingImagePath;
		} elseif (appPath && file_exists(appPath . "/img/image")) {
			path =  \OC_App::getAppWebPath(app) . "/img/image";
		} elseif (appPath && !file_exists(appPath . "/img/basename.svg")
			&& file_exists(appPath . "/img/basename.png")) {
			path =  \OC_App::getAppWebPath(app) . "/img/basename.png";
		} elseif (!empty(app) and file_exists(\OC::SERVERROOT . "/app/img/image")) {
			path =  \OC::WEBROOT . "/app/img/image";
		} elseif (!empty(app) and (!file_exists(\OC::SERVERROOT . "/app/img/basename.svg")
				&& file_exists(\OC::SERVERROOT . "/app/img/basename.png"))) {
			path =  \OC::WEBROOT . "/app/img/basename.png";
		} elseif (file_exists(\OC::SERVERROOT . "/core/img/image")) {
			path =  \OC::WEBROOT . "/core/img/image";
		} elseif (!file_exists(\OC::SERVERROOT . "/core/img/basename.svg")
			&& file_exists(\OC::SERVERROOT . "/core/img/basename.png")) {
			path =  \OC::WEBROOT . "/themes/theme/core/img/basename.png";
		}

		if (path !== '') {
			cache.set(cacheKey, path);
			return path;
		}

		throw new RuntimeException('image not found: image:' . image . ' webroot:' . \OC::WEBROOT . ' serverroot:' . \OC::SERVERROOT);
	}


	/**
	 * Makes an URL absolute
	 * @param string url the url in the ownCloud host
	 * @return string the absolute version of the url
	 */
	public function getAbsoluteURL(string url): string {
		separator = strpos(url, '/') === 0 ? '' : '/';

		if (\OC::CLI && !\defined('PHPUNIT_RUN')) {
			return rtrim(this.config.getSystemValue('overwrite.cli.url'), '/') . '/' . ltrim(url, '/');
		}
		// The ownCloud web root can already be prepended.
		if (\OC::WEBROOT !== '' && strpos(url, \OC::WEBROOT) === 0) {
			url = substr(url, \strlen(\OC::WEBROOT));
		}

		return this.getBaseUrl() . separator . url;
	}

	/**
	 * @param string key
	 * @return string url to the online documentation
	 */
	public function linkToDocs(string key): string {
		theme = \OC::server.getThemingDefaults();
		return theme.buildDocLinkToKey(key);
	}

	/**
	 * @return string base url of the current request
	 */
	public function getBaseUrl(): string {
		return this.request.getServerProtocol() . '://' . this.request.getServerHost() . \OC::WEBROOT;
	}
}
}