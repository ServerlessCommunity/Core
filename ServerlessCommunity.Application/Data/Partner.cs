using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.Application.Data
{
    public class Partner : TableEntity
    {
        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }

        public string Title { get; set; }
        public string Description { get; set; }
        
        public bool IsHighlighted { get; set; }

        public string WebSiteUrl { get; set; }
        public string LogoUrl { get; set; }

        public Partner()
        {
            PartitionKey = string.Empty;
        }
    }
}