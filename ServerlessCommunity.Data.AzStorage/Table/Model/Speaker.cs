using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class Speaker : TableEntity, ISpeaker
    {
        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PhotoUrl { get; set; }

        public bool IsHighlighted { get; set; }

        public string WebSiteUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string FaceBookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string YouTubeUrl { get; set; }
        public string GitHubUrl { get; set; }

        public Speaker()
        {
            PartitionKey = string.Empty;
        }
    }
}