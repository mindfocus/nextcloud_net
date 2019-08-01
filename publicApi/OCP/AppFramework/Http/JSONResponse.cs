using System;
using System.Collections;
using System.Net;
using Newtonsoft.Json.Linq;

namespace OCP.AppFramework.Http
{
/**
 * A renderer for JSON calls
 * @since 6.0.0
 */
    class JSONResponse : Response {

    /**
     * response data
     * @var array|object
     */
    protected object data;


    /**
     * constructor of JSONResponse
     * @param array|object data the object or array that should be transformed
     * @param int statusCode the Http status code, defaults to 200
     * @since 6.0.0
     */
    public JSONResponse(object data= null, HttpStatusCode statusCode=HttpStatusCode.OK) {
        this.data = data;
        this.setStatus(statusCode);
        this.addHeader("Content-Type", "application/json; charset=utf-8");
    }


    /**
     * Returns the rendered json
     * @return string the rendered json
     * @since 6.0.0
     * @throws \Exception If data could not get encoded
     */
    public string render()
    {
        var response = new JObject();
        try
        {
            response = JObject.Parse(this.data.ToString());
        }
        catch (Exception e)
        {
            throw new Exception("Could not json_encode due to invalid " +
            "non UTF-8 characters in the array: {this.data}");
        }
//        response = json_encode(this.data, JSON_HEX_TAG);
//        if(response === false) {
//            
//        }

        return response.ToString();
    }

    /**
     * Sets values in the data json array
     * @param array|object data an array or object which will be transformed
     *                             to JSON
     * @return JSONResponse Reference to this object
     * @since 6.0.0 - return value was added in 7.0.0
     */
    public JSONResponse setData(object data){
        this.data = data;
        return this;
    }


    /**
     * Used to get the set parameters
     * @return array the data
     * @since 6.0.0
     */
    public object getData(){
        return this.data;
    }

    }

}