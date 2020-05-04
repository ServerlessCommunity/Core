import { AzureFunction, Context } from "@azure/functions"

const renderMeetupFunc: AzureFunction = async function (context: Context): Promise<void> {
    let meetup = context.bindings.dataMeetup[0];
    let sessions = context.bindings.dataMeetupSessions;
};

export default renderMeetupFunc;
