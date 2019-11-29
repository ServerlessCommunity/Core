using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessCommunity.Data.AzStorage.Table.Service.Base
{
    public abstract class TableStorageServiceBase<T> where T : ITableEntity, new()
    {
        protected const int FilterSegmentsCount = 15;

        private readonly CloudTable _table;

        protected TableStorageServiceBase(CloudTable table)
        {
            _table = table;
        }

        protected string GenerateCondition(string key, string val, string comparisonOperator = QueryComparisons.Equal)
        {
            return TableQuery.GenerateFilterCondition(
                key,
                comparisonOperator,
                val);
        }
        
        protected string GenerateConditionInt(string key, int val, string comparisonOperator = QueryComparisons.Equal)
        {
            return TableQuery.GenerateFilterConditionForInt(
                key,
                comparisonOperator,
                val);
        }
        
        protected string GenerateConditionBoolean(string key, bool val)
        {
            return TableQuery.GenerateFilterConditionForBool(
                key,
                QueryComparisons.Equal,
                val);
        }

        protected string AppendFilter(string filter, string combineOperator, string newCondition)
        {
            return string.IsNullOrEmpty(filter)
                ? newCondition
                : TableQuery.CombineFilters(filter, combineOperator, newCondition);
        }
        
        protected List<string> SplitFilterForSegments(IEnumerable<string> values, string propertyName)
        {
            var segments = (int) Math.Ceiling(values.Count() / (decimal) FilterSegmentsCount);
            
            var filters = new List<string>(segments);
            for (var i = 0; i < segments; i++)
            {
                var filterIds = values
                    .Skip(i * FilterSegmentsCount)
                    .Take(FilterSegmentsCount)
                    .ToList();

                var filter = string.Empty;
                foreach (var id in filterIds)
                {
                    var condition = GenerateCondition(propertyName, id);
                    filter = AppendFilter(filter, TableOperators.Or, condition);
                }

                filters.Add(filter);
            }

            return filters;
        }
        
        protected async Task<IEnumerable<T>> GetQueryResultsAsync(IEnumerable<string> filters)
        {
            var tasks = filters
                .Select(filter => GetQueryResultsAsync(filter));

            await Task.WhenAll(tasks);

            var results = tasks
                .SelectMany(x => x.Result)
                .ToList();
            
            return results;
        }
        
        protected async Task<IList<T>> GetQueryResultsAsync(string filter = null)
        {
            var results = new List<T>();
            TableContinuationToken token = null;
            
            var query = new TableQuery<T>()
                .Where(filter ?? string.Empty);

            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, token);

                results.AddRange(segment.Results);
                token = segment.ContinuationToken;
            }
            while (token != null);

            return results;
        }

        protected async Task<T> GetEntityById(string partitionKey = null, string rowKey = null)
        {
            var filter = string.Empty;

            if (!string.IsNullOrEmpty(partitionKey))
            {
                filter = AppendFilter(
                    filter, 
                    TableOperators.And, 
                    GenerateCondition(nameof(ITableEntity.PartitionKey), partitionKey));
            }
            
            if (!string.IsNullOrEmpty(rowKey))
            {
                filter = AppendFilter(
                    filter, 
                    TableOperators.And, 
                    GenerateCondition(nameof(ITableEntity.RowKey), rowKey));
            }
            
            return (await GetQueryResultsAsync(filter))
                .SingleOrDefault();
        }
    }
}