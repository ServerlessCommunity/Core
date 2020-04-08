using System;
using Microsoft.WindowsAzure.Storage.Table;
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
        public string DateFormatted => $"{Year:D4}-{Month:D2}-{Day:D2}";
        
        public int TimeStartHours { get; set; }
        public int TimeStartMinutes { get; set; }
        public string TimeStartFormatted => $"{TimeStartHours:D2}:{TimeStartMinutes:D2}";
        
        public int TimeStopHours { get; set; }
        public int TimeStopMinutes { get; set; }
        public string TimeStopFormatted => $"{TimeStopHours:D2}:{TimeStopMinutes:D2}";

        public string Title { get; set; }
        public string Description { get; set; }

        public int? AttendanceFee { get; set; }
        public string AttendanceFeeCurency { get; set; }
        public string AttendanceFeeDescription { get; set; }

        public Guid VenueId { get; set; }
    }
}