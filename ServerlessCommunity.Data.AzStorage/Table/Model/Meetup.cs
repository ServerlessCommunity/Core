using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Data.AzStorage.Table.Model
{
    public class Meetup : TableEntity, IMeetup
    {
        [IgnoreProperty]
        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString("N");
        }
        
        [IgnoreProperty]
        public int Year
        {
            get => int.Parse(PartitionKey);
            set => PartitionKey = value.ToString();
        }
        public int Month { get; set; }
        public int Day { get; set; }

        public string DateFormatted => $"{Year}-{Month}-{Day}";

        public string TimeStart { get; set; }
        public string TimeStop { get; set; }

        public string Title { get; set; }

        public int? AttendanceFee { get; set; }
        public string AttendanceFeeCurency { get; set; }
        public string AttendanceFeeDescription { get; set; }

        public Guid VenueId { get; set; }
    }
}