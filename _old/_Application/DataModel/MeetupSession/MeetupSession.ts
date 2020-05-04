import { DataModelBase } from "../DataModelBase";

export class MeetupSession extends DataModelBase {
    MeetupId: string;
    SessionId: string;
    SpeakerId: string;
    OrderN: number;

    constructor(tableEntity?: any) {
        super(tableEntity);

        if (tableEntity) {
            this.MeetupId = tableEntity.PartitionKey;
        }
    }
}