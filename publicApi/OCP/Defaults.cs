using OC.legacy;

namespace OCP
{
/**
 * public api to access default strings and urls for your templates
 * @since 6.0.0
 */
public class Defaults {

	/**
	 * \OC_Defaults instance to retrieve the defaults
	 * @since 6.0.0
	 */
	private OC_Defaults defaults;

	/**
	 * creates a \OC_Defaults instance which is used in all methods to retrieve the
	 * actual defaults
	 * @since 6.0.0
	 */
	public Defaults(OC_Defaults defaults = null) {
		if (defaults == null) {
			defaults = OC.server.getThemingDefaults();
		}

		this.defaults = defaults;
	}

	/**
	 * get base URL for the organisation behind your ownCloud instance
	 * @return string
	 * @since 6.0.0
	 */
	public string getBaseUrl() {
		return this.defaults.getBaseUrl();
	}

	/**
	 * link to the desktop sync client
	 * @return string
	 * @since 6.0.0
	 */
	public string getSyncClientUrl() {
		return this.defaults.getSyncClientUrl();
	}

	/**
	 * link to the iOS client
	 * @return string
	 * @since 8.0.0
	 */
	public string getiOSClientUrl() {
		return this.defaults.getiOSClientUrl();
	}

	/**
	 * link to the Android client
	 * @return string
	 * @since 8.0.0
	 */
	public string getAndroidClientUrl() {
		return this.defaults.getAndroidClientUrl();
	}

	/**
	 * base URL to the documentation of your ownCloud instance
	 * @return string
	 * @since 6.0.0
	 */
	public string getDocBaseUrl() {
		return this.defaults.getDocBaseUrl();
	}

	/**
	 * name of your ownCloud instance
	 * @return string
	 * @since 6.0.0
	 */
	public string getName() {
		return this.defaults.getName();
	}

	/**
	 * name of your ownCloud instance containing HTML styles
	 * @return string
	 * @since 8.0.0
	 */
	public string getHTMLName() {
		return this.defaults.getHTMLName();
	}

	/**
	 * Entity behind your onwCloud instance
	 * @return string
	 * @since 6.0.0
	 */
	public string getEntity() {
		return this.defaults.getEntity();
	}

	/**
	 * ownCloud slogan
	 * @return string
	 * @since 6.0.0
	 */
	public string getSlogan() {
		return this.defaults.getSlogan();
	}

	/**
	 * logo claim
	 * @return string
	 * @since 6.0.0
	 * @deprecated 13.0.0
	 */
	public string getLogoClaim() {
		return "";
	}

	/**
	 * footer, short version
	 * @return string
	 * @since 6.0.0
	 */
	public string getShortFooter() {
		return this.defaults.getShortFooter();
	}

	/**
	 * footer, long version
	 * @return string
	 * @since 6.0.0
	 */
	public string getLongFooter() {
		return this.defaults.getLongFooter();
	}

	/**
	 * Returns the AppId for the App Store for the iOS Client
	 * @return string AppId
	 * @since 8.0.0
	 */
	public string getiTunesAppId() {
		return this.defaults.getiTunesAppId();
	}

	/**
	 * Themed logo url
	 *
	 * @param bool useSvg Whether to point to the SVG image or a fallback
	 * @return string
	 * @since 12.0.0
	 */
	public string getLogo(bool useSvg = true) {
		return this.defaults.getLogo(useSvg);
	}

	/**
	 * Returns primary color
	 * @return string
	 * @since 12.0.0
	 */
	public string getColorPrimary() {
		return this.defaults.getColorPrimary();
	}

	/**
	 * @param string key
	 * @return string URL to doc with key
	 * @since 12.0.0
	 */
	public string buildDocLinkToKey(string key) {
		return this.defaults.buildDocLinkToKey(key);
	}

	/**
	 * Returns the title
	 * @return string title
	 * @since 12.0.0
	 */
	public string getTitle() {
		return this.defaults.getTitle();
	}

	/**
	 * Returns primary color
	 * @return string
	 * @since 13.0.0
	 */
	public string getTextColorPrimary() {
		return this.defaults.getTextColorPrimary();
	}
}

}