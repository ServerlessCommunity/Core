namespace ServerlessCommunity.Application.ViewModel.Calendar
{
    public class CalendarPage
    {
        public int[] Years { get; set; }
        public CalendarYear[] CalendarYears { get; set; }

        public string PublicUrl => "calendar.html";
        public const string TemplateId = "calendar.hjs";
    }
}