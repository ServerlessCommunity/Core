using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.AzFunc._Extensions
{
    public static class CloudTableExtensions
    {
        public static async Task<List<T>> GetQueryResultsAsync<T>(this CloudTable cloudTable, string partitionKey = null, string rowKey = null)
            where T : ITableEntity, new()
        {
            var filter = string.Empty;

            if (!string.IsNullOrEmpty(partitionKey))
            {
                var condition = GenerateFilter(nameof(ITableEntity.PartitionKey), partitionKey);
                filter = AppendFilter(filter, condition);
            }
            
            if (!string.IsNullOrEmpty(rowKey))
            {
                var condition = GenerateFilter(nameof(ITableEntity.RowKey), rowKey);
                filter = AppendFilter(filter, condition);
            }
            
            var query = new TableQuery<T>().Where(filter);

            return await GetQueryResultsAsync(cloudTable, query);
        }
        
        public static async Task<List<T>> GetHighlightedAsync<T>(this CloudTable cloudTable, string partitionKey = null, string rowKey = null)
            where T : ITableEntity, new()
        {
            var filter = TableQuery.GenerateFilterConditionForBool(
                "IsHighlighted",
                QueryComparisons.Equal,
                true);;

            if (!string.IsNullOrEmpty(partitionKey))
            {
                var condition = GenerateFilter(nameof(ITableEntity.PartitionKey), partitionKey);
                filter = AppendFilter(filter, condition);
            }
            
            if (!string.IsNullOrEmpty(rowKey))
            {
                var condition = GenerateFilter(nameof(ITableEntity.RowKey), rowKey);
                filter = AppendFilter(filter, condition);
            }
            
            var query = new TableQuery<T>().Where(filter);

            return await GetQueryResultsAsync(cloudTable, query);
        }

        private static string GenerateFilter(string key, string val)
        {
            return TableQuery.GenerateFilterCondition(
                key,
                QueryComparisons.Equal,
                val);
        }

        private static string AppendFilter(string filter, string condition)
        {
            return string.IsNullOrEmpty(filter)
                ? condition
                : TableQuery.CombineFilters(filter, TableOperators.And, condition);
        }

        public static async Task<List<T>> GetQueryResultsAsync<T>(this CloudTable cloudTable, TableQuery<T> query)
            where T : ITableEntity, new()
        {
            var results = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, token);

                results.AddRange(segment.Results);
                token = segment.ContinuationToken;
            }
            while (token != null);

            return results;
        }
    }
}