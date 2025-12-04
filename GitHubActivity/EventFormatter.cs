namespace GitHubActivity;


public static class EventFormatter
{
    public static string Format(GitHubEvent ev)
    {
        var repoName = ev.Repo.Name;
        var time = ev.CreatedAt.ToString("MMM d, yyyy");

        return ev.Type switch
        {
            "PushEvent" => $"Pushed {ev.Payload.CommitCount} commit{(ev.Payload.CommitCount == 1 ? "" : "s")} to {repoName}",
            
            "IssuesEvent" when ev.Payload.Action == "opened" => 
                $"Opened a new issue in {repoName}",
            "IssuesEvent" when ev.Payload.Action == "closed" => 
                $"Closed an issue in {repoName}",

            "IssueCommentEvent" => 
                $"Commented on issue #{ev.Payload.Issue?.Number} in {repoName}",

            "PullRequestEvent" when ev.Payload.Action == "opened" => 
                $"Opened a pull request in {repoName}",
            "PullRequestEvent" when ev.Payload.Action == "closed" => 
                $"Closed a pull request in {repoName}",

            "CreateEvent" when ev.Payload.RefType == "repository" => 
                $"Created repository {repoName}",
            "CreateEvent" when ev.Payload.RefType == "branch" => 
                $"Created branch {ev.Payload.Ref?.Replace("refs/heads/", "")} in {repoName}",

            "ForkEvent" => 
                $"Forked {ev.Payload.Forkee?.Name ?? repoName}",

            "WatchEvent" => 
                $"Starred {repoName}",

            "DeleteEvent" => 
                $"Deleted {ev.Payload.RefType} {ev.Payload.Ref} at {repoName}",

            _ => $"Performed {ev.Type.Replace("Event", "")} on {repoName}"
        };
    }
}