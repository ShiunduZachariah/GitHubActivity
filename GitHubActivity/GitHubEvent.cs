using System.Text.Json.Serialization;
namespace GitHubActivity;

public record GitHubEvent(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("actor")] Actor Actor,
    [property: JsonPropertyName("repo")] Repo Repo,
    [property: JsonPropertyName("payload")] Payload Payload,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt
    );
    
public record Actor(
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("display_login")] string DisplayLogin
);

public record Repo(
    [property: JsonPropertyName("name")] string Name
);

// Payload differs per event type – we only model what we use
public record Payload
{
    [property: JsonPropertyName("push_id")] long? PushId { get; init; }
    [property: JsonPropertyName("size")] public int? CommitCount { get; init; }        // for PushEvent
    [property: JsonPropertyName("ref")] public string? Ref { get; init; }              // branch/tag
    [property: JsonPropertyName("ref_type")]
    public string? RefType { get; init; }     // for CreateEvent
    [property: JsonPropertyName("action")] public string? Action { get; init; }       // opened, closed, etc.
    [property: JsonPropertyName("issue")] public Issue? Issue { get; init; }
    [property: JsonPropertyName("pull_request")] PullRequest? PullRequest { get; init; }
    [property: JsonPropertyName("comment")] Comment? Comment { get; init; }
    [property: JsonPropertyName("forkee")] public Repo? Forkee { get; init; }
}

public record Issue([property: JsonPropertyName("number")] int Number);
public record PullRequest([property: JsonPropertyName("number")] int Number);
public record Comment([property: JsonPropertyName("body")] string? Body);