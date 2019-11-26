using System;

namespace ServerlessCommunity.Application.Data
{
    public interface IMeetupPartner
    {
        Guid PartnerId { get; set; }

        Guid MeetupId { get; set; }
    }
}