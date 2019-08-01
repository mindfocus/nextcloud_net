using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ext;

namespace OCP.AppFramework.Http
{
/**
 * Class EmptyContentSecurityPolicy is a simple helper which allows applications
 * to modify the Content-Security-Policy sent by ownCloud. Per default the policy
 * is forbidding everything.
 *
 * As alternative with sane exemptions look at ContentSecurityPolicy
 *
 * @see \OCP\AppFramework\Http\ContentSecurityPolicy
 * @package OCP\AppFramework\Http
 * @since 9.0.0
 */
public class EmptyContentSecurityPolicy {
	/** @var bool Whether inline JS snippets are allowed */
	protected bool? inlineScriptAllowed = null;
	/** @var string Whether JS nonces should be used */
	protected string? _useJsNonce = null;
	/**
	 * @var bool Whether eval in JS scripts is allowed
	 * TODO: Disallow per default
	 * @link https://github.com/owncloud/core/issues/11925
	 */
	protected bool evalScriptAllowed = false;
	/** @var array Domains from which scripts can get loaded */
	protected IList<string> allowedScriptDomains = null;
	/**
	 * @var bool Whether inline CSS is allowed
	 * TODO: Disallow per default
	 * @link https://github.com/owncloud/core/issues/13458
	 */
	protected bool inlineStyleAllowed = false;
	/** @var array Domains from which CSS can get loaded */
	protected IList<string> allowedStyleDomains = null;
	/** @var array Domains from which images can get loaded */
	protected IList<string> allowedImageDomains = null;
	/** @var array Domains to which connections can be done */
	protected IList<string> allowedConnectDomains = null;
	/** @var array Domains from which media elements can be loaded */
	protected IList<string> allowedMediaDomains = null;
	/** @var array Domains from which object elements can be loaded */
	protected IList<string> allowedObjectDomains = null;
	/** @var array Domains from which iframes can be loaded */
	protected IList<string> allowedFrameDomains = null;
	/** @var array Domains from which fonts can be loaded */
	protected IList<string> allowedFontDomains = null;
	/** @var array Domains from which web-workers and nested browsing content can load elements */
	protected IList<string> allowedChildSrcDomains = null;
	/** @var array Domains which can embed this Nextcloud instance */
	protected IList<string> allowedFrameAncestors = null;
	/** @var array Domains from which web-workers can be loaded */
	protected IList<string> allowedWorkerSrcDomains = null;

	/** @var array Locations to report violations to */
	protected IList<string> reportTo = null;

	/**
	 * Whether inline JavaScript snippets are allowed or forbidden
	 * @param bool state
	 * @return this
	 * @since 8.1.0
	 * @deprecated 10.0 CSP tokens are now used
	 */
	public EmptyContentSecurityPolicy allowInlineScript(bool state = false) {
		this.inlineScriptAllowed = state;
		return this;
	}

	/**
	 * Use the according JS nonce
	 *
	 * @param string nonce
	 * @return this
	 * @since 11.0.0
	 */
	public EmptyContentSecurityPolicy useJsNonce(string nonce) {
		this._useJsNonce = nonce;
		return this;
	}

	/**
	 * Whether eval in JavaScript is allowed or forbidden
	 * @param bool state
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy allowEvalScript(bool state = true) {
		this.evalScriptAllowed = state;
		return this;
	}

	/**
	 * Allows to execute JavaScript files from a specific domain. Use * to
	 * allow JavaScript from all domains.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedScriptDomain(string domain) {
		this.allowedScriptDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed script domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowScriptDomain(string domain)
	{
		this.allowedScriptDomains = this.allowedScriptDomains.Except(new List<string>{domain}).ToList();
		return this;
	}

	/**
	 * Whether inline CSS snippets are allowed or forbidden
	 * @param bool state
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy allowInlineStyle(bool state = true) {
		this.inlineStyleAllowed = state;
		return this;
	}

	/**
	 * Allows to execute CSS files from a specific domain. Use * to allow
	 * CSS from all domains.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedStyleDomain(string domain) {
		this.allowedStyleDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed style domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowStyleDomain(string domain)
	{
		this.allowedStyleDomains = this.allowedStyleDomains.Except(new List<string>() {domain}).ToList();
		return this;
	}

	/**
	 * Allows using fonts from a specific domain. Use * to allow
	 * fonts from all domains.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedFontDomain(string domain) {
		this.allowedFontDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed font domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowFontDomain(string domain)
	{
		this.allowedFontDomains = this.allowedFontDomains.Except(new List<string>() {domain}).ToList();
		return this;
	}

	/**
	 * Allows embedding images from a specific domain. Use * to allow
	 * images from all domains.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedImageDomain(string domain) {
		this.allowedImageDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed image domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowImageDomain(string domain)
	{
		this.allowedImageDomains = this.allowedImageDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * To which remote domains the JS connect to.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedConnectDomain(string domain) {
		this.allowedConnectDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed connect domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowConnectDomain(string domain)
	{
		this.allowedConnectDomains = this.allowedConnectDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * From which domains media elements can be embedded.
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedMediaDomain(string domain) {
		this.allowedMediaDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed media domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowMediaDomain(string domain)
	{
		this.allowedMediaDomains = this.allowedMediaDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * From which domains objects such as <object>, <embed> or <applet> are executed
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedObjectDomain(string domain) {
		this.allowedObjectDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed object domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowObjectDomain(string domain)
	{
		this.allowedObjectDomains = this.allowedObjectDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * Which domains can be embedded in an iframe
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy addAllowedFrameDomain(string domain) {
		this.allowedFrameDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed frame domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 */
	public EmptyContentSecurityPolicy disallowFrameDomain(string domain) {
		this.allowedFrameDomains = this.allowedFrameDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * Domains from which web-workers and nested browsing content can load elements
	 * @param string domain Domain to whitelist. Any passed value needs to be properly sanitized.
	 * @return this
	 * @since 8.1.0
	 * @deprecated 15.0.0 use addAllowedWorkerSrcDomains or addAllowedFrameDomain
	 */
	public EmptyContentSecurityPolicy addAllowedChildSrcDomain(string domain) {
		this.allowedChildSrcDomains.Add(domain);
		return this;
	}

	/**
	 * Remove the specified allowed child src domain from the allowed domains.
	 *
	 * @param string domain
	 * @return this
	 * @since 8.1.0
	 * @deprecated 15.0.0 use the WorkerSrcDomains or FrameDomain
	 */
	public EmptyContentSecurityPolicy disallowChildSrcDomain(string domain) {
		this.allowedChildSrcDomains = this.allowedChildSrcDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * Domains which can embed an iFrame of the Nextcloud instance
	 *
	 * @param string domain
	 * @return this
	 * @since 13.0.0
	 */
	public EmptyContentSecurityPolicy addAllowedFrameAncestorDomain(string domain) {
		this.allowedFrameAncestors.Add(domain);
		return this;
	}

	/**
	 * Domains which can embed an iFrame of the Nextcloud instance
	 *
	 * @param string domain
	 * @return this
	 * @since 13.0.0
	 */
	public EmptyContentSecurityPolicy disallowFrameAncestorDomain(string domain) {
		this.allowedFrameAncestors = this.allowedFrameAncestors.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * Domain from which workers can be loaded
	 *
	 * @param string domain
	 * @return this
	 * @since 15.0.0
	 */
	public EmptyContentSecurityPolicy addAllowedWorkerSrcDomain(string domain) {
		this.allowedWorkerSrcDomains.Add(domain);
		return this;
	}

	/**
	 * Remove domain from which workers can be loaded
	 *
	 * @param string domain
	 * @return this
	 * @since 15.0.0
	 */
	public EmptyContentSecurityPolicy disallowWorkerSrcDomain(string domain) {
		this.allowedWorkerSrcDomains = this.allowedWorkerSrcDomains.Except(new List<string>(1) {domain}).ToList();
		return this;
	}

	/**
	 * Add location to report CSP violations to
	 *
	 * @param string location
	 * @return this
	 * @since 15.0.0
	 */
	public EmptyContentSecurityPolicy addReportTo(string location) {
		this.reportTo.Add(location);
		return this;
	}

	/**
	 * Get the generated Content-Security-Policy as a string
	 * @return string
	 * @since 8.1.0
	 */
	public string buildPolicy() {
		var policy = "default-src 'none';";
		policy += "base-uri 'none';";
		policy += "manifest-src 'self';";

		if(this.allowedScriptDomains.IsNotEmpty() || this.inlineScriptAllowed.HasValue || this.evalScriptAllowed) {
			policy += "script-src ";
			if(this._useJsNonce != null) {
				policy += "\'nonce-'.base64_encode(this.useJsNonce).'\'";
				this.allowedScriptDomains.Remove("\'self\'");
				if(allowedScriptDomains.Count != 0) {
					policy += " ";
				}
			}
			if(this.allowedScriptDomains != null)
			{
				policy += String.Join(" ", this.allowedScriptDomains);
			}
			if(this.inlineScriptAllowed != null) {
				policy += " \'unsafe-inline\'";
			}
			if(this.evalScriptAllowed) {
				policy += " \'unsafe-eval\'";
			}
			policy += ";";
		}

		if(this.allowedStyleDomains.Any() || this.inlineStyleAllowed) {
			policy += "style-src ";
			if(this.allowedStyleDomains != null)
			{
				policy += String.Join(" ", this.allowedStyleDomains);
			}
			if(this.inlineStyleAllowed) {
				policy += " \'unsafe-inline\'";
			}
			policy += ";";
		}

		if(this.allowedImageDomains.Any())
		{
			policy += "img-src " + string.Join(" ", this.allowedImageDomains);
			policy += ";";
		}

		if(this.allowedFontDomains.Any())
		{
			policy += "font-src " + String.Join(" ", this.allowedFontDomains);
			policy += ";";
		}

		if(this.allowedConnectDomains.Any())
		{
			policy += "connect-src " + String.Join(" ", this.allowedConnectDomains);
			policy += ";";
		}

		if(this.allowedMediaDomains.Any()) {
			policy += "media-src " + String.Join(" ", this.allowedMediaDomains);
			policy += ";";
		}

		if(this.allowedObjectDomains.Any())
		{
			policy += "object-src " + String.Join(" ", this.allowedObjectDomains);
			policy += ";";
		}

		if(this.allowedFrameDomains.Any())
		{
			policy += "frame-src " + String.Join(" ", this.allowedFrameDomains);
			policy += ";";
		}
		
		if(this.allowedChildSrcDomains.Any())
		{
			policy += "child-src " + String.Join(" ", this.allowedChildSrcDomains);
			policy += ";";
		}

		if(this.allowedFrameAncestors.Any())
		{
			policy += "frame-ancestors " + String.Join(" ", this.allowedFrameAncestors);
			policy += ";";
		}
		
		if(this.allowedWorkerSrcDomains.Any())
		{
			policy += "worker-src  " + String.Join(" ", this.allowedWorkerSrcDomains);
			policy += ";";
		}
		
		if(this.reportTo.Any())
		{
			policy += "report-uri  " + String.Join(" ", this.reportTo);
			policy += ";";
		}

		return policy.TrimEnd(';');
	}
}

}