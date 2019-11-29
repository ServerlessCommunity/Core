using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class MeetupSessionMaterial : TableEntity, IMeetupSessionMaterial
    {
        [IgnoreProperty]
        public Guid MeetupId
        {
            get => Guid.Parse(PartitionKey);
            set => PartitionKey = value.ToString("N");
        }

        public Guid SessionId { get; set; }

        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }

        public int OrderN { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public MeetupSessionMaterial()
        {
            Id = Guid.NewGuid();
        }
    }
}