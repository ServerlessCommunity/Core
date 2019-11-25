using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.Application.Data
{
    public class MeetupPartner : TableEntity
    {
        [IgnoreProperty]
        public Guid Id
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
    }
}