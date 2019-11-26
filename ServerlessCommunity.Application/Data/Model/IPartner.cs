using System;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IPartner
    {
        Guid Id { get; set; }

        string Title { get; set; }
        string Description { get; set; }
        
        bool IsHighlighted { get; set; }

        string WebSiteUrl { get; set; }
        string LogoUrl { get; set; }
    }
}