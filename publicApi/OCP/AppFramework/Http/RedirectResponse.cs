using System.Net;

namespace OCP.AppFramework.Http
{
/**
 * Redirects to a different URL
 * @since 7.0.0
 */
    public class RedirectResponse : Response {

    private string redirectURL;

    /**
     * Creates a response that redirects to a url
     * @param string redirectURL the url to redirect to
     * @since 7.0.0
     */
    public RedirectResponse(string redirectURL) {
        this.redirectURL = redirectURL;
        this.setStatus(HttpStatusCode.Redirect);
        this.addHeader("Location", redirectURL);
    }


    /**
     * @return string the url to redirect
     * @since 7.0.0
     */
    public string getRedirectURL() {
        return this.redirectURL;
    }


    }

}