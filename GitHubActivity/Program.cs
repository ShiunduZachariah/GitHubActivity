using System.Net.Http.Headers;
using System.Text.Json;
using GitHubActivity;

if (args.Length == 0 || args[0] is "--help" or "-h")
{
    PrintHelp();
    return;
}

string username = args[0].Trim();

if (string.IsNullOrWhiteSpace(username) || username.Contains('/'))
{
    Console.WriteLine("Error: Invalid GitHub username.");
    return;
}

await FetchAndDisplayActivity(username);
return;   // ← important after await

static async Task FetchAndDisplayActivity(string username)
{
    string url = $"https://api.github.com/users/{username}/events";

    HttpResponseMessage response;
    try
    {
        response = await GitHubClient.GetAsync(url);
    }
    catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
    {
        Console.WriteLine("Network error – check your internet connection.");
        return;
    }

    if (!response.IsSuccessStatusCode)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            Console.WriteLine($"User '{username}' not found.");
        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            Console.WriteLine("GitHub rate limit exceeded – try again in a minute.");
        else
            Console.WriteLine($"GitHub error: {(int)response.StatusCode} {response.ReasonPhrase}");
        return;
    }

    string json = await response.Content.ReadAsStringAsync();

    var events = JsonSerializer.Deserialize<GitHubEvent[]>(json);

    if (events == null || events.Length == 0)
    {
        Console.WriteLine("No public activity found.");
        return;
    }

    Console.WriteLine($"Recent activity for {username}:\n");

    foreach (var ev in events.Take(30)) // GitHub returns max 30
    {
        string message = EventFormatter.Format(ev);
        Console.WriteLine($"• {message}");
    }

    Console.WriteLine($"\nShowing {events.Length} most recent events.");
}

void PrintHelp()
{
    Console.WriteLine(@"GitHub Activity CLI

Usage:
  github-activity <username>
  github-activity --help

Examples:
  github-activity torvalds
  github-activity kamranahmedse
  github-activity octocat");
}
