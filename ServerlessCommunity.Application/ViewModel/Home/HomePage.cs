using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.ViewModel.Meetup;

namespace ServerlessCommunity.Application.ViewModel.Home
{
    public class HomePage
    {
        public MeetupPage[] Meetups { get; set; }

        public Speaker[] Speakers { get; set; }

        public Partner[] Partners { get; set; }
        
        public string PublicUrl => "index.html";
        public const string TemplateId = "index.hjs";
    }
}