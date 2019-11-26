using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class Session : TableEntity, ISession
    {
        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }
        
        [IgnoreProperty]
        public Guid SpeakerId
        {
            get => Guid.Parse(PartitionKey);
            set => PartitionKey = value.ToString("N");
        }
        
        public string Title { get; set; }
        public string Description { get; set; }

        public Session()
        {
            PartitionKey = string.Empty;
        }
    }
}