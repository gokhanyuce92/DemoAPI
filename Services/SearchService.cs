using Demo.Interfaces;
using Demo.Models;
using Nest;

namespace Demo.Services
{
    public class SearchService<T> : ISearchService<T> where T : class
    {
        private readonly IElasticClient _elasticClient;
        public SearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Result<string>> AddDocumentAsync(string? indexName, T document)
        {
            indexName ??= _elasticClient.ConnectionSettings.DefaultIndex;

            var response = await _elasticClient.IndexAsync(document, idx => idx.Index(indexName));

            if (response.IsValid)
            {
                return new Result<string>{ IsSuccess = true, Data = response.Id };
            }
            return new Result<string>{ IsSuccess = false, ErrorMessage = response.OriginalException?.Message };
        }

        public async Task<bool> CreateIndexAsync(string indexName)
        {
            var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                .Map<T>(m => m
                    .AutoMap()
                )
            );
            return createIndexResponse.IsValid;
        }

        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            var deleteIndexResponse = await _elasticClient.Indices.DeleteAsync(indexName);
            
            return deleteIndexResponse.IsValid;
        }

        public async Task<Result<string>> DeleteDocumentAsync(string? indexName, string id)
        {
            indexName ??= _elasticClient.ConnectionSettings.DefaultIndex;

            var deleteResponse = await _elasticClient.DeleteAsync(new DeleteRequest(indexName, id));
            
            if (deleteResponse.IsValid)
            {
                return new Result<string>{ IsSuccess = true, Data = "Document deleted successfully." };
            }
            return new Result<string>{ IsSuccess = false, ErrorMessage = deleteResponse.OriginalException?.Message };
        }

        public async Task<Result<IEnumerable<T>>> GetAllDocumentsAsync(string? indexName, int size = 1000)
        {
            indexName ??= _elasticClient.ConnectionSettings.DefaultIndex;

            var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Query(q => q.MatchAll())
                .Size(size)
            );

            if (response.IsValid)
            {
                return new Result<IEnumerable<T>>{ IsSuccess = true, Data = response.Documents };
            }
            return new Result<IEnumerable<T>>{ IsSuccess = false, ErrorMessage = response.OriginalException?.Message };
        }
    }
}