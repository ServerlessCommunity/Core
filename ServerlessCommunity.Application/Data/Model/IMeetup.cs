using System;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IMeetup
    {
        Guid Id { get; set; }

        int Year { get; set; }
        int Month { get; set; }
        int Day { get; set; }

        string DateFormatted { get; }

        string TimeStart { get; set; }
        string TimeStop { get; set; }

        string Title { get; set; }

        int? AttendanceFee { get; set; }
        string AttendanceFeeCurency { get; set; }
        string AttendanceFeeDescription { get; set; }

        Guid VenueId { get; set; }
    }
}