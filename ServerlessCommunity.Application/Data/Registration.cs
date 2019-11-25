using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.Application.Data
{
    public class Registration : TableEntity
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

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsSubscribed { get; set; }
        public bool IsConfirmed { get; set; }
    }
}