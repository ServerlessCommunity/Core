using ServerlessCommunity.Application.ViewModel.Meetup;

namespace ServerlessCommunity.Application.ViewModel.Calendar
{
    public class CalendarYear
    {
        public int Year { get; set; }
        public MeetupPage[] Meetups { get; set; }
    }
}