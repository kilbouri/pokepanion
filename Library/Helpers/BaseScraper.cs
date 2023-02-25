using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Pokepanion.Library.Extensions;

namespace Pokepanion.Library.Helpers;

public abstract class BaseScraper<T> {

    private static readonly HttpClient client = new();

    public async Task<IEnumerable<T>> ScrapeValues(string baseUrl, string endpoint) {
        var rootPage = await GetHtmlDocumentAsync(baseUrl, endpoint);
        return await ScrapeAsync(rootPage, baseUrl);
    }

    protected abstract Task<IEnumerable<T>> ScrapeAsync(HtmlDocument page, string baseUrl);

    protected static async Task<HtmlDocument> GetHtmlDocumentAsync(string baseUrl, string endpoint) {
        string baseUrlNoEndSlash = baseUrl.EndsWith('/') ? baseUrl[..^1] : baseUrl;
        string endpointNoStartSlash = endpoint.StartsWith('/') ? endpoint[1..] : endpoint;
        string target = baseUrlNoEndSlash + '/' + endpointNoStartSlash;

        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, target));

        if (response.StatusCode != HttpStatusCode.OK) {
            throw new RequestFailedException(target);
        }

        return response.Content.ReadAsHtmlDocument();
    }

    private class RequestFailedException : Exception {
        public RequestFailedException(string uri)
            : base($"'{uri}' returned a status code that was not {HttpStatusCode.OK}") { }
    }

}
