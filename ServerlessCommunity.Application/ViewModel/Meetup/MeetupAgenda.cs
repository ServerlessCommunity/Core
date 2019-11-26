using ServerlessCommunity.Application.Data;

namespace ServerlessCommunity.Application.ViewModel.Meetup
{
    public class MeetupAgenda
    {
        public MeetupSession MeetupSession { get; set; }
        public Session Session { get; set; }
        public Speaker Speaker { get; set; }
    }
}