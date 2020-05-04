import { AzureFunction, Context } from "@azure/functions"

import { Command } from "./../_Application/Command/Command"

const commandReceiverFunc: AzureFunction = async function (context: Context, commandItem: Command): Promise<void> {
    
};

export default commandReceiverFunc;
