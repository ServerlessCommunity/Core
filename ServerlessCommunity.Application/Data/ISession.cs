using System;

namespace ServerlessCommunity.Application.Data
{
    public interface ISession
    {
        Guid Id { get; set; }

        Guid SpeakerId { get; set; }

        string Title { get; set; }
        string Description { get; set; }
    }
}