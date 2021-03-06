using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OCP;
using OCP.AppFramework.Http;
using OCP.Http.Client;

namespace OCP.AppFramework
{
/**
 * Base class to inherit your controllers from
 * @since 6.0.0
 */
    public abstract class Controller
    {
        /**
         * app name
         * @var string
         * @since 7.0.0
         */
        protected string appName;

        /**
         * current request
         * @var .OCP.IRequest
         * @since 6.0.0
         */
        protected OCP.IRequest request;

        /**
         * @var array
         * @since 7.0.0
         */
        private IDictionary<string, Func<Response, Response>> responders;

        /**
         * constructor of the controller
         * @param string appName the name of the app
         * @param IRequest request an instance of the request
         * @since 6.0.0 - parameter appName was added in 7.0.0 - parameter app was removed in 7.0.0
         */
        public Controller(string appName,
            IRequest request)
        {
            this.appName = appName;
            this.request = request;

            // default responders
            this.responders.Add("json", data =>
            {
                if (data is DataResponse)
                {
                    var response = new JSONResponse(((DataResponse) data).getData(),
                        ((DataResponse) data).getStatus());
                    var dataHeaders = data.getHeaders();
                    var headers = response.getHeaders();
                    if (dataHeaders.ContainsKey("Content-Type"))
                    {
                        headers.Remove("Content-Type");
                    }

                    response.setHeaders(dataHeaders.Concat(headers).ToDictionary(o => o.Key, p => p.Value));
                    return response;
                }

                return new JSONResponse(data);
            });
        }


        /**
         * Parses an HTTP accept header and returns the supported responder type
         * @param string acceptHeader
         * @param string default
         * @return string the responder type
         * @since 7.0.0
         * @since 9.1.0 Added default parameter
         */
        public string getResponderByHTTPHeader(string acceptHeader, string @default = "json")
        {
            var headers = acceptHeader.Split(',');

            // return the first matching responder
            foreach (var header in headers)
            {
                var headerLower = header.Trim().ToLower();

                var responder = headerLower.Replace("application/", "");
                if (this.responders.ContainsKey(responder))
                {
                    return responder;
                }
            }

            // no matching header return default
            return @default;
        }


        /**
         * Registers a formatter for a type
         * @param string format
         * @param .Closure responder
         * @since 7.0.0
         */
        protected void registerResponder(string format, Func<Response, Response> responder)
        {
            this.responders[format] = responder;
        }


        /**
         * Serializes and formats a response
         * @param mixed response the value that was returned from a controller and
         * is not a Response instance
         * @param string format the format for which a formatter has been registered
         * @throws .DomainException if format does not match a registered formatter
         * @return Response
         * @since 7.0.0
         */
        public Response buildResponse(Response response, string format = "json")
        {
            if (responders.ContainsKey(format))
            {
                var responder = this.responders[format];
                return responder(response);
            }

            throw new Exception("No responder registered for format " +
                                format + "!");
        }
    }
}