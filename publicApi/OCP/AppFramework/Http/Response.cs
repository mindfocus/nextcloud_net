using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ext;
using OCP.AppFramework.Utility;
using OCP.Security;

namespace OCP.AppFramework.Http
{
/**
 * Base class for responses. Also used to just send headers.
 *
 * It handles headers, HTTP status code, last modified and ETag.
 * @since 6.0.0
 */
public class Response {

	/**
	 * Headers - defaults to ['Cache-Control' => 'no-cache, no-store, must-revalidate']
	 * @var array
	 */
	private IDictionary<string,string> headers = new Dictionary<string, string>
	{
		{"Cache-Control", "no-cache, no-store, must-revalidate"}
	};


	/**
	 * Cookies that will be need to be constructed as header
	 * @var array
	 */
	private IDictionary<string,string> cookies = new Dictionary<string, string>();


	/**
	 * HTTP status code - defaults to STATUS OK
	 * @var int
	 */
	private HttpStatusCode status = HttpStatusCode.OK;


	/**
	 * Last modified date
	 * @var \DateTime
	 */
	private DateTime? lastModified = null;


	/**
	 * ETag
	 * @var string
	 */
	private string ETag;

	/** @var ContentSecurityPolicy|null Used Content-Security-Policy */
	private ContentSecurityPolicyManager contentSecurityPolicy = null;

	/** @var bool */
	private bool throttled = false;
	/** @var array */
	private IList<string> throttleMetadata = new List<string>();

	/**
	 * Caches the response
	 * @param int cacheSeconds the amount of seconds that should be cached
	 * if 0 then caching will be disabled
	 * @return this
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public Response cacheFor(int cacheSeconds) {
		if(cacheSeconds > 0)
		{
			this.addHeader("Cache-Control", $"max-age={cacheSeconds},must-revalidate");
			// Old scool prama caching
			this.addHeader("Pragma", "public");
			// Set expires header
			var expires = new DateTime();
			/** @var ITimeFactory time */
			var time = OC.server.query(typeof(ITimeFactory));
			expires.setTimestamp(time.getTime());
			expires.add(new \DateInterval('PT'.cacheSeconds.'S'));
			this.addHeader('Expires', expires.format(\DateTime::RFC2822));
		} else {
			this.addHeader("Cache-Control", "no-cache, no-store, must-revalidate");
			this.headers.Remove("Expires");
			this.headers.Remove("Pragma");
		}

		return this;
	}

	/**
	 * Adds a new cookie to the response
	 * @param string name The name of the cookie
	 * @param string value The value of the cookie
	 * @param \DateTime|null expireDate Date on that the cookie should expire, if set
	 * 									to null cookie will be considered as session
	 * 									cookie.
	 * @return this
	 * @since 8.0.0
	 */
	public Response addCookie(name, value, \DateTime expireDate = null) {
		this.cookies[name] = array('value' => value, 'expireDate' => expireDate);
		return this;
	}


	/**
	 * Set the specified cookies
	 * @param array cookies array('foo' => array('value' => 'bar', 'expire' => null))
	 * @return this
	 * @since 8.0.0
	 */
	public Response setCookies(IDictionary<string,string> cookies) {
		this.cookies = cookies;
		return this;
	}


	/**
	 * Invalidates the specified cookie
	 * @param string name
	 * @return this
	 * @since 8.0.0
	 */
	public Response invalidateCookie(string name) {
		this.addCookie(name, "expired", new DateTime("1971-01-01 00:00"));
		return this;
	}

	/**
	 * Invalidates the specified cookies
	 * @param array cookieNames array('foo', 'bar')
	 * @return this
	 * @since 8.0.0
	 */
	public function invalidateCookies(array cookieNames) {
		foreach(cookieNames as cookieName) {
			this.invalidateCookie(cookieName);
		}
		return this;
	}

	/**
	 * Returns the cookies
	 * @return array
	 * @since 8.0.0
	 */
	public function getCookies() {
		return this.cookies;
	}

	/**
	 * Adds a new header to the response that will be called before the render
	 * function
	 * @param string name The name of the HTTP header
	 * @param string value The value, null will delete it
	 * @return this
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public Response addHeader(string name, string value) {
		name = name.Trim();  // always remove leading and trailing whitespace
		                      // to be able to reliably check for security
		                      // headers

		if(value.IsEmpty())
		{
			this.headers.Remove(name);
		} else {
			this.headers[name] = value;
		}

		return this;
	}


	/**
	 * Set the headers
	 * @param array headers value header pairs
	 * @return this
	 * @since 8.0.0
	 */
	public Response setHeaders(IDictionary<string,string> headers) {
		this.headers = headers;

		return this;
	}


	/**
	 * Returns the set headers
	 * @return array the headers
	 * @since 6.0.0
	 */
	public IDictionary<string,string> getHeaders() {
		var mergeWith = new Dictionary<string,string>();

		if(this.lastModified != null)
		{
			mergeWith["Last-Modified"] =
				this.lastModified?.ToString("R"); // format(\DateTime::RFC2822);
		}

		// Build Content-Security-Policy and use default if none has been specified
		if (this.contentSecurityPolicy == null)
		{
			this.setContentSecurityPolicy(new ContentSecurityPolicy());
		}
		this.headers["Content-Security-Policy"] = this.contentSecurityPolicy.buildPolicy();

		if(this.ETag.IsNotEmpty()) {
			mergeWith["ETag"] = "\"" + this.ETag + "\"";
		}

		return mergeWith.Concat(this.headers).ToDictionary(o => o.Key, p => p.Value);
	}


	/**
	 * By default renders no output
	 * @return string
	 * @since 6.0.0
	 */
	public string render() {
		return "";
	}


	/**
	 * Set response status
	 * @param int status a HTTP status code, see also the STATUS constants
	 * @return Response Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public Response setStatus(HttpStatusCode status) {
		this.status = status;

		return this;
	}

	/**
	 * Set a Content-Security-Policy
	 * @param EmptyContentSecurityPolicy csp Policy to set for the response object
	 * @return this
	 * @since 8.1.0
	 */
	public Response setContentSecurityPolicy(EmptyContentSecurityPolicy csp) {
		this.contentSecurityPolicy = csp;
		return this;
	}

	/**
	 * Get the currently used Content-Security-Policy
	 * @return EmptyContentSecurityPolicy|null Used Content-Security-Policy or null if
	 *                                    none specified.
	 * @since 8.1.0
	 */
	public ContentSecurityPolicyManager getContentSecurityPolicy() {
		return this.contentSecurityPolicy;
	}


	/**
	 * Get response status
	 * @since 6.0.0
	 */
	public HttpStatusCode getStatus() {
		return this.status;
	}


	/**
	 * Get the ETag
	 * @return string the etag
	 * @since 6.0.0
	 */
	public string getETag() {
		return this.ETag;
	}


	/**
	 * Get "last modified" date
	 * @return \DateTime RFC2822 formatted last modified date
	 * @since 6.0.0
	 */
	public DateTime? getLastModified() {
		return this.lastModified;
	}


	/**
	 * Set the ETag
	 * @param string ETag
	 * @return Response Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public Response setETag(string ETag) {
		this.ETag = ETag;
		return this;
	}


	/**
	 * Set "last modified" date
	 * @param \DateTime lastModified
	 * @return Response Reference to this object
	 * @since 6.0.0 - return value was added in 7.0.0
	 */
	public Response setLastModified(DateTime lastModified) {
		this.lastModified = lastModified;
		return this;
	}

	/**
	 * Marks the response as to throttle. Will be throttled when the
	 * @BruteForceProtection annotation is added.
	 *
	 * @param array metadata
	 * @since 12.0.0
	 */
	public void throttle(IList<string>  metadata ) {
		this.throttled = true;
		this.throttleMetadata = metadata;
	}

	/**
	 * Returns the throttle metadata, defaults to empty array
	 *
	 * @return array
	 * @since 13.0.0
	 */
	public IList<string>  getThrottleMetadata() {
		return this.throttleMetadata;
	}

	/**
	 * Whether the current response is throttled.
	 *
	 * @since 12.0.0
	 */
	public bool isThrottled() {
		return this.throttled;
	}
}
}