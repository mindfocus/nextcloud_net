namespace OC.legacy
{
    public class OC_Defaults
    {
        private theme;

	private defaultEntity;
	private defaultName;
	private defaultTitle;
	private defaultBaseUrl;
	private defaultSyncClientUrl;
	private defaultiOSClientUrl;
	private defaultiTunesAppId;
	private defaultAndroidClientUrl;
	private defaultDocBaseUrl;
	private defaultDocVersion;
	private defaultSlogan;
	private defaultColorPrimary;
	private defaultTextColorPrimary;

	public function __construct() {
		l10n = .OC.server.getL10N("lib");
		config = .OC.server.getConfig();

		this.defaultEntity = "Nextcloud"; /* e.g. company name, used for footers and copyright notices */
		this.defaultName = "Nextcloud"; /* short name, used when referring to the software */
		this.defaultTitle = "Nextcloud"; /* can be a longer name, for titles */
		this.defaultBaseUrl = "https://nextcloud.com";
		this.defaultSyncClientUrl = config.getSystemValue("customclient_desktop", "https://nextcloud.com/install/#install-clients");
		this.defaultiOSClientUrl = config.getSystemValue("customclient_ios", "https://geo.itunes.apple.com/us/app/nextcloud/id1125420102?mt=8");
		this.defaultiTunesAppId = config.getSystemValue("customclient_ios_appid", "1125420102");
		this.defaultAndroidClientUrl = config.getSystemValue("customclient_android", "https://play.google.com/store/apps/details?id=com.nextcloud.client");
		this.defaultDocBaseUrl = "https://docs.nextcloud.com";
		this.defaultDocVersion = .OC_Util.getVersion()[0]; // used to generate doc links
		this.defaultSlogan = l10n.t("a safe home for all your data");
		this.defaultColorPrimary = "#0082c9";
		this.defaultTextColorPrimary = "#ffffff";

		themePath = OC.SERVERROOT . "/themes/" . OC_Util.getTheme() . "/defaults.php";
		if (file_exists(themePath)) {
			// prevent defaults.php from printing output
			ob_start();
			require_once themePath;
			ob_end_clean();
			if (class_exists("OC_Theme")) {
				this.theme = new OC_Theme();
			}
		}
	}

	/**
	 * @param string method
	 */
	private function themeExist(method) {
		if (isset(this.theme) && method_exists(this.theme, method)) {
			return true;
		}
		return false;
	}

	/**
	 * Returns the base URL
	 * @return string URL
	 */
	public function getBaseUrl() {
		if (this.themeExist("getBaseUrl")) {
			return this.theme.getBaseUrl();
		} else {
			return this.defaultBaseUrl;
		}
	}

	/**
	 * Returns the URL where the sync clients are listed
	 * @return string URL
	 */
	public string getSyncClientUrl() {
		if (this.themeExist("getSyncClientUrl")) {
			return this.theme.getSyncClientUrl();
		} else {
			return this.defaultSyncClientUrl;
		}
	}

	/**
	 * Returns the URL to the App Store for the iOS Client
	 * @return string URL
	 */
	public function getiOSClientUrl() {
		if (this.themeExist("getiOSClientUrl")) {
			return this.theme.getiOSClientUrl();
		} else {
			return this.defaultiOSClientUrl;
		}
	}

	/**
	 * Returns the AppId for the App Store for the iOS Client
	 * @return string AppId
	 */
	public function getiTunesAppId() {
		if (this.themeExist("getiTunesAppId")) {
			return this.theme.getiTunesAppId();
		} else {
			return this.defaultiTunesAppId;
		}
	}

	/**
	 * Returns the URL to Google Play for the Android Client
	 * @return string URL
	 */
	public function getAndroidClientUrl() {
		if (this.themeExist("getAndroidClientUrl")) {
			return this.theme.getAndroidClientUrl();
		} else {
			return this.defaultAndroidClientUrl;
		}
	}

	/**
	 * Returns the documentation URL
	 * @return string URL
	 */
	public function getDocBaseUrl() {
		if (this.themeExist("getDocBaseUrl")) {
			return this.theme.getDocBaseUrl();
		} else {
			return this.defaultDocBaseUrl;
		}
	}

	/**
	 * Returns the title
	 * @return string title
	 */
	public function getTitle() {
		if (this.themeExist("getTitle")) {
			return this.theme.getTitle();
		} else {
			return this.defaultTitle;
		}
	}

	/**
	 * Returns the short name of the software
	 * @return string title
	 */
	public function getName() {
		if (this.themeExist("getName")) {
			return this.theme.getName();
		} else {
			return this.defaultName;
		}
	}

	/**
	 * Returns the short name of the software containing HTML strings
	 * @return string title
	 */
	public function getHTMLName() {
		if (this.themeExist("getHTMLName")) {
			return this.theme.getHTMLName();
		} else {
			return this.defaultName;
		}
	}

	/**
	 * Returns entity (e.g. company name) - used for footer, copyright
	 * @return string entity name
	 */
	public function getEntity() {
		if (this.themeExist("getEntity")) {
			return this.theme.getEntity();
		} else {
			return this.defaultEntity;
		}
	}

	/**
	 * Returns slogan
	 * @return string slogan
	 */
	public function getSlogan() {
		if (this.themeExist("getSlogan")) {
			return this.theme.getSlogan();
		} else {
			return this.defaultSlogan;
		}
	}

	/**
	 * Returns logo claim
	 * @return string logo claim
	 * @deprecated 13.0.0
	 */
	public function getLogoClaim() {
		return "";
	}

	/**
	 * Returns short version of the footer
	 * @return string short footer
	 */
	public function getShortFooter() {
		if (this.themeExist("getShortFooter")) {
			footer = this.theme.getShortFooter();
		} else {
			footer = "<a href="". this.getBaseUrl() . "" target="_blank"" .
				" rel="noreferrer noopener">" .this.getEntity() . "</a>".
				" – " . this.getSlogan();
		}

		return footer;
	}

	/**
	 * Returns long version of the footer
	 * @return string long footer
	 */
	public function getLongFooter() {
		if (this.themeExist("getLongFooter")) {
			footer = this.theme.getLongFooter();
		} else {
			footer = this.getShortFooter();
		}

		return footer;
	}

	/**
	 * @param string key
	 * @return string URL to doc with key
	 */
	public function buildDocLinkToKey(key) {
		if (this.themeExist("buildDocLinkToKey")) {
			return this.theme.buildDocLinkToKey(key);
		}
		return this.getDocBaseUrl() . "/server/" . this.defaultDocVersion . "/go.php?to=" . key;
	}

	/**
	 * Returns primary color
	 * @return string
	 */
	public function getColorPrimary() {

		if (this.themeExist("getColorPrimary")) {
			return this.theme.getColorPrimary();
		}
		if (this.themeExist("getMailHeaderColor")) {
			return this.theme.getMailHeaderColor();
		}
		return this.defaultColorPrimary;
	}

	/**
	 * @return array scss variables to overwrite
	 */
	public function getScssVariables() {
		if(this.themeExist("getScssVariables")) {
			return this.theme.getScssVariables();
		}
		return [];
	}

	public function shouldReplaceIcons() {
		return false;
	}

	/**
	 * Themed logo url
	 *
	 * @param bool useSvg Whether to point to the SVG image or a fallback
	 * @return string
	 */
	public function getLogo(useSvg = true) {
		if (this.themeExist("getLogo")) {
			return this.theme.getLogo(useSvg);
		}

		if(useSvg) {
			logo = .OC.server.getURLGenerator().imagePath("core", "logo/logo.svg");
		} else {
			logo = .OC.server.getURLGenerator().imagePath("core", "logo/logo.png");
		}
	    return logo . "?v=" . hash("sha1", implode(".", .OCP.Util.getVersion()));
	}

	public function getTextColorPrimary() {
		if (this.themeExist("getTextColorPrimary")) {
			return this.theme.getTextColorPrimary();
		}
		return this.defaultTextColorPrimary;
	}
    }
}