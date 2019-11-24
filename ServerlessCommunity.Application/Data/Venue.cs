using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.Application.Data
{
    public class Venue : TableEntity
    {
        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }
        
        public string Title { get; set; }
        public string Address { get; set; }
        public string MapUrl { get; set; }

        public Venue()
        {
            PartitionKey = string.Empty;
        }
    }
}