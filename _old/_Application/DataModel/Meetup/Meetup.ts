import { DataModelBase } from "../DataModelBase";

export class Meetup extends DataModelBase {
    VenueId: String;
    Title: String;
    Year: Number;
    Month: Number;
    Day: Number;
    TimeStart: string;
    TimeFinish: string;

    constructor(tableEntity?: any) {
        super(tableEntity);

        if (tableEntity) {
            this.Year = tableEntity.PartitionKey;
        }
    }
}