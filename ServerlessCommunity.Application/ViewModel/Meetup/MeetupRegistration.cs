namespace ServerlessCommunity.Application.ViewModel.Meetup
{
    public class MeetupRegistration
    {
        public bool IsOpened { get; set; }

        // This quick hotfix is required because there is an issue with processing boolean value
        public string IsOpened2 => IsOpened ? "True" : null;
        public string Url { get; set; }
    }
}