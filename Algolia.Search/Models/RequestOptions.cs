using System;
using System.Collections.Generic;

namespace Algolia.Search.Models
{
    public class RequestOptions
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryParams = new Dictionary<string, string>();
        private string _forwardedFor;

        public RequestOptions()
        {

        }

        public RequestOptions SetForwardedFor(string forwardedFor)
        {
            _forwardedFor = forwardedFor;
            return this;
        }

        public RequestOptions AddExtraHeader(string key, string value)
        {
            _headers[key] = value;
            return this;
        }

        public RequestOptions AddExtraQueryParameters(string key, string value)
        {
            _queryParams[key] = value;
            return this;
        }

        public Dictionary<String, String> GenerateExtraHeaders()
        {
            if (_forwardedFor != null)
            {
                _headers["X-Forwarded-For"] = _forwardedFor;
            }
            return _headers;
        }

        public Dictionary<string, string> GenerateExtraQueryParams()
        {
            return _queryParams;
        }

        public override string ToString()
        {
            return "RequestOptions{" +
                   "headers=" + _headers +
                   ", queryParams=" + _queryParams +
                   ", forwardedFor='" + _forwardedFor + '\'' +
                   '}';
        }
    }
}
