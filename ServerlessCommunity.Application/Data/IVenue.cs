using System;

namespace ServerlessCommunity.Application.Data
{
    public interface IVenue
    {
        Guid Id { get; set; }

        string Title { get; set; }
        string Address { get; set; }
        string MapUrl { get; set; }
    }
}