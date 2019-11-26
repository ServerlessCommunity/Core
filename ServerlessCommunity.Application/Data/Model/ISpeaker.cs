using System;
using ServerlessCommunity.Application.Data.Model.Base;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface ISpeaker: IHighlighted
    {
        Guid Id { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string Bio { get; set; }
        string PhotoUrl { get; set; }

        string WebSiteUrl { get; set; }
        string LinkedInUrl { get; set; }
        string FaceBookUrl { get; set; }
        string TwitterUrl { get; set; }
        string YouTubeUrl { get; set; }
        string GitHubUrl { get; set; }
    }
}