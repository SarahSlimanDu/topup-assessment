using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mime;
using System.Text;

namespace Commons.HttpRequestBuild
{
    public class HttpRequestBuilder
    {
        protected readonly HttpRequestMessage _request;

        public HttpRequestBuilder(string baseUri)
        {
            this._request = new()
            {
                RequestUri = new Uri(baseUri)
            };
        }

        public virtual HttpRequestBuilder AddRequestHeader(string name, string value)
        {
            this._request.Headers.Add(name, value);
            return this;
        }

        public HttpRequestBuilder AddQueryString(KeyValuePair<string, string> query)
        {
            var uri = QueryHelpers.AddQueryString(this._request.RequestUri?.ToString() ?? "", new Dictionary<string, string?> { { query.Key, query.Value } });
            this._request.RequestUri = new Uri(uri);
            return this;
        }

        public HttpRequestBuilder AddQueryString(string key, string value)
        {
            var uri = QueryHelpers.AddQueryString(this._request.RequestUri?.ToString() ?? "", new Dictionary<string, string?> { { key, value } });
            this._request.RequestUri = new Uri(uri);
            return this;
        }

        public virtual HttpRequestBuilder SetRequestBody(string body, Encoding? encoding = null, string mediaType = MediaTypeNames.Application.Json)
        {
            this._request.Content = new StringContent(body, encoding ?? Encoding.UTF8, mediaType);
            return this;
        }

        public virtual HttpRequestBuilder AddRequestPath(string path = "/")
        {
            if (this._request.RequestUri is null) this._request.RequestUri = new Uri(path);
            else this._request.RequestUri = new Uri(this._request.RequestUri, path);
            return this;
        }

        public virtual HttpRequestBuilder SetRequestMethod(HttpMethod method)
        {
            this._request.Method = method;
            return this;
        }
        public virtual HttpRequestMessage Build() => this._request;

        public static HttpRequestBuilder Create(string baseUri) => new(baseUri);

        public static HttpRequestBuilder Create(string baseUri, HttpMethod method) => new HttpRequestBuilder(baseUri).SetRequestMethod(method);

        public static HttpRequestBuilder Create(string baseUri, string path, HttpMethod method) => new HttpRequestBuilder(baseUri).SetRequestMethod(method).AddRequestPath(path);
    }
}
