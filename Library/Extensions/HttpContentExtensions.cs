using System.Net.Http;
using HtmlAgilityPack;

namespace Pokepanion.Library.Extensions;

public static class HttpContentExtensions {

    public static string ReadAsString(this HttpContent response) {
        var readTask = response.ReadAsStringAsync();
        readTask.Wait();

        return readTask.Result;
    }

    public static HtmlDocument ReadAsHtmlDocument(this HttpContent response) {
        HtmlDocument result = new();
        result.Load(response.ReadAsStream());

        return result;
    }
}
