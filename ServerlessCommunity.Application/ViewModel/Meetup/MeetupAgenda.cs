using ServerlessCommunity.Application.Data;

namespace ServerlessCommunity.Application.ViewModel.Meetup
{
    public class MeetupAgenda
    {
        public IMeetupSession MeetupSession { get; set; }
        public ISession Session { get; set; }
        public ISpeaker Speaker { get; set; }
    }
}