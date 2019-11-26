using System;

namespace ServerlessCommunity.Application.Data
{
    public interface IRegistration
    {
        Guid Id { get; set; }

        Guid MeetupId { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }

        bool IsSubscribed { get; set; }
        bool IsConfirmed { get; set; }
    }
}