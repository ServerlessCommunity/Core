using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class MeetupPartner : TableEntity, IMeetupPartner
    {
        [IgnoreProperty]
        public Guid PartnerId
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