using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class Venue : TableEntity, IVenue
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