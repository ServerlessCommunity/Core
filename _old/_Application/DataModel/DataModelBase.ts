export abstract class DataModelBase {
    Id: string;

    constructor(tableEntity?: any) {
        if (tableEntity) {
            this.Id = tableEntity.RowKey;

            Object
                .keys(tableEntity)
                .filter(key => { return !["RowKey", "PartitionKey"].includes(key); })
                .forEach(key => { this[key] = tableEntity[key]; });
        }
    }
}