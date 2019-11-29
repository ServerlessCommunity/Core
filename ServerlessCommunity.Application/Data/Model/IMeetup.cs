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

        int TimeStartHours { get; set; }
        int TimeStartMinutes { get; set; }
        string TimeStartFormatted { get; }

        int TimeStopHours { get; set; }
        int TimeStopMinutes { get; set; }
        string TimeStopFormatted { get; }

        string Title { get; set; }

        int? AttendanceFee { get; set; }
        string AttendanceFeeCurency { get; set; }
        string AttendanceFeeDescription { get; set; }

        Guid VenueId { get; set; }
    }
}