namespace OCP.AppFramework.Http
{
/**
 * Redirects to a different URL
 * @since 7.0.0
 */
    class RedirectResponse : Response {

    private redirectURL;

    /**
     * Creates a response that redirects to a url
     * @param string redirectURL the url to redirect to
     * @since 7.0.0
     */
    public function __construct(redirectURL) {
        this.redirectURL = redirectURL;
        this.setStatus(Http::STATUS_SEE_OTHER);
        this.addHeader('Location', redirectURL);
    }


    /**
     * @return string the url to redirect
     * @since 7.0.0
     */
    public function getRedirectURL() {
        return this.redirectURL;
    }


    }

}