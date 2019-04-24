namespace OCP.AppFramework.Http
{
/**
 * Class StrictInlineContentSecurityPolicy is a simple helper which allows applications to
 * modify the Content-Security-Policy sent by Nextcloud. Per default only JavaScript,
 * stylesheets, images, fonts, media and connections from the same domain
 * ('self') are allowed.
 *
 * Even if a value gets modified above defaults will still get appended. Please
 * notice that Nextcloud ships already with sensible defaults and those policies
 * should require no modification at all for most use-cases.
 *
 * This is a temp helper class from the default ContentSecurityPolicy to allow slow
 * migration to a stricter CSP. This does not allow inline styles.
 *
 * @package OCP\AppFramework\Http
 * @since 14.0.0
 */
    class StrictInlineContentSecurityPolicy : ContentSecurityPolicy {

    /**
     * @since 14.0.0
     */
    public function __construct() {
        this->inlineStyleAllowed = false;
    }
    }

}