using OCP.AppFramework.Http;

namespace OCP.AppFramework
{
/**
 * Base class to inherit your controllers from that are used for RESTful APIs
 * @since 8.1.0
 */
abstract class OCSController : ApiController {

	/** @var int */
	private int ocsVersion;

	/**
	 * constructor of the controller
	 * @param string appName the name of the app
	 * @param IRequest request an instance of the request
	 * @param string corsMethods comma separated string of HTTP verbs which
	 * should be allowed for websites or webapps when calling your API, defaults to
	 * 'PUT, POST, GET, DELETE, PATCH'
	 * @param string corsAllowedHeaders comma separated string of HTTP headers
	 * which should be allowed for websites or webapps when calling your API,
	 * defaults to 'Authorization, Content-Type, Accept'
	 * @param int corsMaxAge number in seconds how long a preflighted OPTIONS
	 * request should be cached, defaults to 1728000 seconds
	 * @since 8.1.0
	 */
	public OCSController(string appName,
								IRequest request,
								string corsMethods='PUT, POST, GET, DELETE, PATCH',
								string corsAllowedHeaders='Authorization, Content-Type, Accept',
								int corsMaxAge=1728000) : base(appName, request, corsMethods,corsAllowedHeaders , corsMaxAge){
		parent::__construct(appName, request, corsMethods,
							corsAllowedHeaders, corsMaxAge);
		this.registerResponder('json', function (data) {
			return this.buildOCSResponse('json', data);
		});
		this.registerResponder('xml', function (data) {
			return this.buildOCSResponse('xml', data);
		});
	}

	/**
	 * @param int version
	 * @since 11.0.0
	 * @internal
	 */
	public function setOCSVersion(version) {
		this.ocsVersion = version;
	}

	/**
	 * Since the OCS endpoints default to XML we need to find out the format
	 * again
	 * @param mixed response the value that was returned from a controller and
	 * is not a Response instance
	 * @param string format the format for which a formatter has been registered
	 * @throws \DomainException if format does not match a registered formatter
	 * @return Response
	 * @since 9.1.0
	 */
	public Response buildResponse(response, format = 'xml') {
		return buildResponse(response, format);
	}

	/**
	 * Unwrap data and build ocs response
	 * @param string format json or xml
	 * @param DataResponse data the data which should be transformed
	 * @since 8.1.0
	 * @return \OC\AppFramework\OCS\BaseResponse
	 */
	private function buildOCSResponse(format, DataResponse data) {
		if (this.ocsVersion === 1) {
			return new \OC\AppFramework\OCS\V1Response(data, format);
		}
		return new \OC\AppFramework\OCS\V2Response(data, format);
	}

}

}