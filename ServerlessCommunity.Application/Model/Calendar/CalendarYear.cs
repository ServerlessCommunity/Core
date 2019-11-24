using ServerlessCommunity.Application.Model.Meetup;

namespace ServerlessCommunity.Application.Model.Calendar
{
    public class CalendarYear
    {
        public int Year { get; set; }
        public MeetupPage[] Meetups { get; set; }
    }
}