using System;
using ServerlessCommunity.Application.Data.Model.Base;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IPartner : IHighlighted
    {
        Guid Id { get; set; }

        string Title { get; set; }
        string Description { get; set; }
        
        string WebSiteUrl { get; set; }
        string LogoUrl { get; set; }
    }
}