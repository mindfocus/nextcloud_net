using System.Collections;
using System.Collections.Generic;

namespace OCP.AppFramework.Http
{
    /**
     * Class ContentSecurityPolicy is a simple helper which allows applications to
     * modify the Content-Security-Policy sent by Nextcloud. Per default only JavaScript,
     * stylesheets, images, fonts, media and connections from the same domain
     * ('self') are allowed.
     *
     * Even if a value gets modified above defaults will still get appended. Please
     * notice that Nextcloud ships already with sensible defaults and those policies
     * should require no modification at all for most use-cases.
     *
     * This class allows unsafe-eval of javascript and unsafe-inline of CSS.
     *
     * @package OCP\AppFramework\Http
     * @since 8.1.0
     */
    public class ContentSecurityPolicy : EmptyContentSecurityPolicy
    {
        /** @var bool Whether inline JS snippets are allowed */
        protected bool inlineScriptAllowed = false;

        /** @var bool Whether eval in JS scripts is allowed */
        protected bool evalScriptAllowed = false;

        /** @var array Domains from which scripts can get loaded */
        protected IList<string> allowedScriptDomains = new List<string>
        {
            "\"self\""
        };

        /**
         * @var bool Whether inline CSS is allowed
         * TODO: Disallow per default
         * @link https://github.com/owncloud/core/issues/13458
         */
        protected bool inlineStyleAllowed = true;

        /** @var array Domains from which CSS can get loaded */
        protected IList<string> allowedStyleDomains = new List<string>
        {
            "\"self\""
        };

        /** @var array Domains from which images can get loaded */
        protected IList<string> allowedImageDomains = new List<string>
        {
            "\"self\"",
            "data:",
            "blob:",
        };

        /** @var array Domains to which connections can be done */
        protected IList<string> allowedConnectDomains = new List<string>
        {
            "\"self\""
        };

        /** @var array Domains from which media elements can be loaded */
        protected IList<string> allowedMediaDomains = new List<string>
        {
            "\"self\""
        };

        /** @var array Domains from which object elements can be loaded */
        protected IList<string> allowedObjectDomains = new List<string>();

        /** @var array Domains from which iframes can be loaded */
        protected IList<string> allowedFrameDomains = new List<string>();

        /** @var array Domains from which fonts can be loaded */
        protected IList<string> allowedFontDomains = new List<string>
        {
            "\"self\"",
            "data:"
        };

        /** @var array Domains from which web-workers and nested browsing content can load elements */
        protected IList<string> allowedChildSrcDomains = new List<string>();

        /** @var array Domains which can embed this Nextcloud instance */
        protected IList<string> allowedFrameAncestors = new List<string>
        {
            "\"self\""
        };

        /** @var array Domains from which web-workers can be loaded */
        protected IList<string> allowedWorkerSrcDomains = new List<string>();

        /** @var array Locations to report violations to */
        protected IList<string> reportTo = new List<string>();
    }
}