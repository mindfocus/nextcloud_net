using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OCP.AppFramework.Http
{
/**
 * A generic DataResponse class that is used to return generic data responses
 * for responders to transform
 * @since 8.0.0
 */
    public class DataResponse : Response {

    /**
     * response data
     * @var array|object
     */
    protected object data;


    /**
     * @param array|object data the object or array that should be transformed
     * @param int statusCode the Http status code, defaults to 200
     * @param array headers additional key value based headers
     * @since 8.0.0
     */
    public DataResponse(object data, HttpStatusCode statusCode=HttpStatusCode.OK,
    IDictionary<string,string> headers = null) {
        this.data = data;
        this.setStatus(statusCode);
        if (headers != null)
        {
            this.setHeaders(this.getHeaders().Concat(headers).ToDictionary(o => o.Key, p=>p.Value));
        }
    }


    /**
     * Sets values in the data json array
     * @param array|object data an array or object which will be transformed
     * @return DataResponse Reference to this object
     * @since 8.0.0
     */
    public DataResponse setData(object data){
        this.data = data;

        return this;
    }


    /**
     * Used to get the set parameters
     * @return array the data
     * @since 8.0.0
     */
    public object getData(){
        return this.data;
    }

    }

}