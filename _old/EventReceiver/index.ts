import { AzureFunction, Context } from "@azure/functions"

import { Event } from "../_Application/Event/Event";
import { EventType } from "../_Application/Event/EventType";

const eventReceiverFunc: AzureFunction = async function (context: Context, eventItem: Event): Promise<void> {
    switch (eventItem.type) {
        case EventType.MeetupNew:
            break;
        case EventType.MeetupClose:
            break;
    }
};

export default eventReceiverFunc;
