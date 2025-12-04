using System.Net.Http;
using System.Net.Http.Headers;

namespace GitHubActivity;

public static class GitHubClient
{
    private static readonly HttpClient _client = new HttpClient();

    static GitHubClient()
    {
        // REQUIRED by GitHub – otherwise you get 403 instantly
        _client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("GitHubActivityCLI", "1.0"));

        // Be nice to GitHub – accept JSON
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    }

    public static Task<HttpResponseMessage> GetAsync(string requestUri) =>
        _client.GetAsync(requestUri);
}