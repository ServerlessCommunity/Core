using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class MeetupSession : TableEntity, IMeetupSession
    {
        [IgnoreProperty]
        public Guid SessionId
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }
        
        [IgnoreProperty]
        public Guid MeetupId
        {
            get => Guid.Parse(PartitionKey);
            set => PartitionKey = value.ToString("N");
        }
        
        public int OrderN { get; set; }
    }
}