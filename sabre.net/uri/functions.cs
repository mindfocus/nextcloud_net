using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace sabre.net.uri
{
/**
 * This file contains all the uri handling functions.
 *
 * @copyright Copyright (C) fruux GmbH (https://fruux.com/)
 * @author Evert Pot (http://evertpot.com/)
 * @license http://sabre.io/license/
 */
class functions
{
    
/**
 * Resolves relative urls, like a browser would.
 *
 * This function takes a basePath, which itself _may_ also be relative, and
 * then applies the relative path on top of it.
 *
 * @throws InvalidUriException
 */
string resolve(string basePath, string newPath) 
{
    var delta = parse(newPath);

    // If the new path defines a scheme, it"s absolute and we can just return
    // that.
    if (delta.ContainsKey("scheme")) {
        return build(delta);
    }

    var @base = parse(basePath);
    Func<string, string> pick = part =>
    {
        if (delta.ContainsKey(part))
        {
            return delta[part];
        }

        if (@base.ContainsKey(part))
        {
            return @base[part];
        }
        return null;
    };

    var newParts = new Dictionary<string, string>();

    newParts["scheme"] = pick("scheme");
    newParts["host"] = pick("host");
    newParts["port"] = pick("port");

    var path = "";
    if (delta["path"] != null && delta["path"].Length > 0) {
        // If the path starts with a slash
        if ('/' == delta["path"][0]) {
            path = delta["path"];
        } else {
            // Removing last component from base path.
            path = @base["path"];
            var length = path.IndexOf("/") + 1;
            if (-1 != length)
            {
                path = path.Substring(0, length);
            }
            path += "/"  + delta["path"];
        }
    } else {
        // path = @base["path"] ?: "/";
        path = @base["path"];
    }
    // Removing .. and .
    var pathParts = path.Split('/'); // explode("/", path);
    var newPathParts = new Queue<string>();
    foreach (var pathPart in pathParts ) {
        switch (pathPart) {
            //case "" :
            case ".":
                break;
            case "..":
                newPathParts.Dequeue();
                break;
            default:
                newPathParts.Enqueue(pathPart);
                break;
        }
    }

    path = String.Join("/", newPathParts); // implode("/", newPathParts);

    // If the source url ended with a /, we want to preserve that.
    newParts["path"] = path;
    if (delta.ContainsKey("query")) {
        newParts["query"] = delta["query"];
    } else if (@base["query"] != "" && delta["host"] == "" && delta["path"] == "") {
        // Keep the old query if host and path didn"t change
        newParts["query"] = @base["query"];
    }
    if (delta.ContainsKey("fragment")) {
        newParts["fragment"] = delta["fragment"];
    }

    return build(newParts);
}

/**
 * Takes a URI or partial URI as its argument, and normalizes it.
 *
 * After normalizing a URI, you can safely compare it to other URIs.
 * This function will for instance convert a %7E into a tilde, according to
 * rfc3986.
 *
 * It will also change a %3a into a %3A.
 *
 * @throws InvalidUriException
 */
string normalize(string uri)
{
    var parts = parse(uri);

    if (parts["path"] != "")
    {
        var pathParts = parts["path"].Trim().Split('/'); // explode("/", ltrim(parts["path"], "/"));
        var newPathParts = new Queue<string>();
        foreach (var pathPart in pathParts) {
            switch (pathPart) {
                case ".":
                    // skip
                    break;
                case "..":
                    // One level up in the hierarchy
                    newPathParts.Dequeue();
                    break;
                default:
                    // Ensuring that everything is correctly percent-encoded.
                    newPathParts.Enqueue(HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(pathPart)));
                    // newPathParts.Enqueue(rawurlencode(rawurldecode(pathPart)));
                    break;
            }
        }
        // parts["path"] = "/".implode("/", newPathParts);
        parts["path"] = "/" + String.Join("/", newPathParts); 
    }

    if (parts.ContainsKey("scheme")) {
        parts["scheme"] = (parts["scheme"].ToLower());
        var defaultPorts = new Dictionary<string, string>
        {
            {
                "http", "80"
            },
            {
                "https", "443"
            }
        };

        if (parts["port"] != "" && defaultPorts.ContainsKey(parts["scheme"]) && defaultPorts[parts["scheme"]] == parts["port"]) {
            // Removing default ports.
            parts.Remove("port");
        }
        // A few HTTP specific rules.
        switch (parts["scheme"]) {
            case "http":
            case "https":
                if (parts["path"] == "") {
                    // An empty path is equivalent to / in http.
                    parts["path"] = "/";
                }
                break;
        }
    }

    if (parts.ContainsKey("host")) {
        parts["host"] = parts["host"].ToLower();
    }

    return build(parts);
}

/**
 * Parses a URI and returns its individual components.
 *
 * This method largely behaves the same as PHP"s parse_url, except that it will
 * return an array with all the array keys, including the ones that are not
 * set by parse_url, which makes it a bit easier to work with.
 *
 * Unlike PHP"s parse_url, it will also convert any non-ascii characters to
 * percent-encoded strings. PHP"s parse_url corrupts these characters on OS X.
 *
 * @return array<string, string>
 *
 * @throws InvalidUriException
 */
IDictionary<string, string> parse(string uri)
{
    // Normally a URI must be ASCII, however. However, often it"s not and
    // parse_url might corrupt these strings.
    //
    // For that reason we take any non-ascii characters from the uri and
    // uriencode them first.
    Regex re = new Regex( @"/[^[:ascii:]]/u" );
    uri = re.Replace(uri, match =>
    {
        return HttpUtility.UrlEncode(match.Value);
    });
    

    var result = _parse_fallback(uri);
        result.Add("scheme", null);
        result.Add("host", null);
        result.Add("path", null);
        result.Add("port", null);
        result.Add("user", null);
        result.Add("query", null);
        result.Add("fragment", null);
        return result;
}

/**
 * This function takes the components returned from PHP"s parse_url, and uses
 * it to generate a new uri.
 *
 * @param array<string, int|string> parts
 */
string build(IDictionary<string, string> parts)
{
    var uri = "";

    var authority = "";
    if (parts["host"] != "") {
        authority = parts["host"];
        if (parts["user"] != "") {
            authority = parts["user"] + "@" + authority;
        }
        if (parts["port"] != "") {
            authority = authority + ":" + parts["port"];
        }
    }

    if (parts["scheme"] != "") {
        // If there"s a scheme, there"s also a host.
        uri = parts["scheme"] + ":";
    }
    if (authority != "" || (parts["scheme"] != "" && "file" == parts["scheme"])) {
        // No scheme, but there is a host.
        uri += "//"  + authority;
    }

    if (parts["path"] != "") {
        uri += parts["path"];
    }
    if (parts["query"] != "") {
        uri += "?" +  parts["query"];
    }
    if (parts["fragment"] != "") {
        uri += "#" +  parts["fragment"];
    }

    return uri;
}

/**
 * Returns the "dirname" and "basename" for a path.
 *
 * The reason there is a custom function for this purpose, is because
 * basename() is locale aware (behaviour changes if C locale or a UTF-8 locale
 * is used) and we need a method that just operates on UTF-8 characters.
 *
 * In addition basename and dirname are platform aware, and will treat
 * backslash (\) as a directory separator on windows.
 *
 * This method returns the 2 components as an array.
 *
 * If there is no dirname, it will return an empty string. Any / appearing at
 * the end of the string is stripped off.
 *
 * @return array<int, mixed>
 */
IDictionary<int, object> split(string path)
{
    var result = Regex.Matches(path, @"/^(?:(?:(.*)(?:\/+))?([^\/]+))(?:\/?)/u");
    if (result.Count > 2)
    {
        return new Dictionary<int, object>{{0, result[0]}, {1, result[1]}};
    }

    return new Dictionary<int, object>{{0, null}, {1, null}};
}

/**
 * This function is another implementation of parse_url, except this one is
 * fully written in PHP.
 *
 * The reason is that the PHP bug team is not willing to admit that there are
 * bugs in the parse_url implementation.
 *
 * This function is only called if the main parse method fails. It"s pretty
 * crude and probably slow, so the original parse_url is usually preferred.
 *
 * @return array<string, mixed>
 *
 * @throws InvalidUriException
 */
IDictionary<string, object> _parse_fallback(string uri)
{
    // Normally a URI must be ASCII, however. However, often it"s not and
    // parse_url might corrupt these strings.
    //
    // For that reason we take any non-ascii characters from the uri and
    // uriencode them first.
    uri = preg_replace_callback(
        "/[^[:ascii:]]/u",
        function (matches) {
            return rawurlencode(matches[0]);
        },
        uri
    );

    result = [
        "scheme" => null,
        "host" => null,
        "port" => null,
        "user" => null,
        "path" => null,
        "fragment" => null,
        "query" => null,
    ];

    if (preg_match("% ^([A-Za-z][A-Za-z0-9+-\.]+): %x", uri, matches)) {
        result["scheme"] = matches[1];
        // Take what"s left.
        uri = substr(uri, strlen(result["scheme"]) + 1);
    }

    // Taking off a fragment part
    if (false !== strpos(uri, "#")) {
        list(uri, result["fragment"]) = explode("#", uri, 2);
    }
    // Taking off the query part
    if (false !== strpos(uri, "?")) {
        list(uri, result["query"]) = explode("?", uri, 2);
    }

    if ("///" == substr(uri, 0, 3)) {
        // The triple slash uris are a bit unusual, but we have special handling
        // for them.
        result["path"] = substr(uri, 2);
        result["host"] = "";
    } elseif ("//" == substr(uri, 0, 2)) {
        // Uris that have an authority part.
        regex = "
          %^
            //
            (?: (?<user> [^:@]+) (: (?<pass> [^@]+)) @)?
            (?<host> ( [^:/]* | \[ [^\]]+ \] ))
            (?: : (?<port> [0-9]+))?
            (?<path> / .*)?
          %x
        ";
        if (!preg_match(regex, uri, matches)) {
            throw new InvalidUriException("Invalid, or could not parse URI");
        }
        if (matches["host"]) {
            result["host"] = matches["host"];
        }
        if (isset(matches["port"])) {
            result["port"] = (int) matches["port"];
        }
        if (isset(matches["path"])) {
            result["path"] = matches["path"];
        }
        if (matches["user"]) {
            result["user"] = matches["user"];
        }
        if (matches["pass"]) {
            result["pass"] = matches["pass"];
        }
    } else {
        result["path"] = uri;
    }

    return result;
}
}