using ServerlessCommunity.Application.Data;

namespace ServerlessCommunity.Application.ViewModel.Meetup
{
    public class MeetupPage
    {
        public Data.Meetup Meetup { get; set; }
        public Venue Venue { get; set; }
        public MeetupAgenda[] Sessions { get; set; }
        public MeetupRegistration Registration { get; set; }
        public Partner[] Partners { get; set; }

        public string PublicUrl => $"meetup/{Meetup.Year:D4}-{Meetup.Month:D2}-{Meetup.Day:D2}.html";
        public const string TemplateId = "meetup.hjs";
    }
}