using System;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IMeetupPartner
    {
        Guid PartnerId { get; set; }

        Guid MeetupId { get; set; }
    }
}