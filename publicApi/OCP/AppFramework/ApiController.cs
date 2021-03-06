using OCP.AppFramework.Http;

namespace OCP.AppFramework
{
/**
 * Base class to inherit your controllers from that are used for RESTful APIs
 * @since 7.0.0
 */
abstract class ApiController : Controller {

    private string corsMethods;
    private string corsAllowedHeaders;
    private int corsMaxAge;

    /**
     * constructor of the controller
     * @param string appName the name of the app
     * @param IRequest request an instance of the request
     * @param string corsMethods comma separated string of HTTP verbs which
     * should be allowed for websites or webapps when calling your API, defaults to
     * "PUT, POST, GET, DELETE, PATCH"
     * @param string corsAllowedHeaders comma separated string of HTTP headers
     * which should be allowed for websites or webapps when calling your API,
     * defaults to "Authorization, Content-Type, Accept"
     * @param int corsMaxAge number in seconds how long a preflighted OPTIONS
     * request should be cached, defaults to 1728000 seconds
	 * @since 7.0.0
     */
    public ApiController(string appName,
                                IRequest request,
                                string corsMethods="PUT, POST, GET, DELETE, PATCH",
                                string corsAllowedHeaders="Authorization, Content-Type, Accept",
                                int corsMaxAge=1728000) : base(appName, request)
    {
        this.corsMethods = corsMethods;
        this.corsAllowedHeaders = corsAllowedHeaders;
        this.corsMaxAge = corsMaxAge;
    }


    /**
     * This method implements a preflighted cors response for you that you can
     * link to for the options request
     *
     * @NoAdminRequired
     * @NoCSRFRequired
     * @PublicPage
	 * @since 7.0.0
     */
    public Response preflightedCors() {
        if(isset(this.request.server["HTTP_ORIGIN"])) {
            origin = this.request.server["HTTP_ORIGIN"];
        } else {
            origin = "*";
        }

        var response = new Response();
        response.addHeader("Access-Control-Allow-Origin", origin);
        response.addHeader("Access-Control-Allow-Methods", this.corsMethods);
        response.addHeader("Access-Control-Max-Age", this.corsMaxAge);
        response.addHeader("Access-Control-Allow-Headers", this.corsAllowedHeaders);
        response.addHeader("Access-Control-Allow-Credentials", "false");
        return response;
    }


}

}