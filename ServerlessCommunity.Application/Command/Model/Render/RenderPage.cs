namespace ServerlessCommunity.Application.Command.Render
{
    public class RenderPage : CommandBase
    {
        public string DataInstanceId { get; set; }
        public string PublicUrl { get; set; }
        public string TemplateId { get; set; }
    }
}
