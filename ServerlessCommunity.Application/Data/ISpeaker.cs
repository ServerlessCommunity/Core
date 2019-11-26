using System;

namespace ServerlessCommunity.Application.Data
{
    public interface ISpeaker
    {
        Guid Id { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string Bio { get; set; }
        string PhotoUrl { get; set; }

        bool IsHighlighted { get; set; }

        string WebSiteUrl { get; set; }
        string LinkedInUrl { get; set; }
        string FaceBookUrl { get; set; }
        string TwitterUrl { get; set; }
        string YouTubeUrl { get; set; }
        string GitHubUrl { get; set; }
    }
}