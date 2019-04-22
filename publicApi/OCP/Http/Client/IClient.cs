using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Http.Client
{
    /**
     * Interface IClient
     *
     * @package OCP\Http
     * @since 8.1.0
     */
    public interface IClient
    {

        /**
         * Sends a GET request
         * @param string $uri
         * @param array $options Array such as
         *              'query' => [
         *                  'field' => 'abc',
         *                  'other_field' => '123',
         *                  'file_name' => fopen('/path/to/file', 'r'),
         *              ],
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse get(string uri, PhpArray options);

        /**
         * Sends a HEAD request
         * @param string $uri
         * @param array $options Array such as
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse head(string uri, PhpArray options);

        /**
         * Sends a POST request
         * @param string $uri
         * @param array $options Array such as
         *              'body' => [
         *                  'field' => 'abc',
         *                  'other_field' => '123',
         *                  'file_name' => fopen('/path/to/file', 'r'),
         *              ],
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse post(string uri, PhpArray options);

        /**
         * Sends a PUT request
         * @param string $uri
         * @param array $options Array such as
         *              'body' => [
         *                  'field' => 'abc',
         *                  'other_field' => '123',
         *                  'file_name' => fopen('/path/to/file', 'r'),
         *              ],
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse put(string uri, PhpArray options);

        /**
         * Sends a DELETE request
         * @param string $uri
         * @param array $options Array such as
         *              'body' => [
         *                  'field' => 'abc',
         *                  'other_field' => '123',
         *                  'file_name' => fopen('/path/to/file', 'r'),
         *              ],
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse delete(string uri, PhpArray options);

        /**
         * Sends a options request
         * @param string $uri
         * @param array $options Array such as
         *              'body' => [
         *                  'field' => 'abc',
         *                  'other_field' => '123',
         *                  'file_name' => fopen('/path/to/file', 'r'),
         *              ],
         *              'headers' => [
         *                  'foo' => 'bar',
         *              ],
         *              'cookies' => ['
         *                  'foo' => 'bar',
         *              ],
         *              'allow_redirects' => [
         *                   'max'       => 10,  // allow at most 10 redirects.
         *                   'strict'    => true,     // use "strict" RFC compliant redirects.
         *                   'referer'   => true,     // add a Referer header
         *                   'protocols' => ['https'] // only allow https URLs
         *              ],
         *              'save_to' => '/path/to/file', // save to a file or a stream
         *              'verify' => true, // bool or string to CA file
         *              'debug' => true,
         * @return IResponse
         * @throws \Exception If the request could not get completed
         * @since 8.1.0
         */
        IResponse options(string uri, PhpArray options);
}

}
