using Demo.Models;

namespace Demo.Interfaces {
    public interface ISearchService<T> where T : class
    {
        Task<Result<IEnumerable<T>>> GetAllDocumentsAsync(string? indexName, int size = 1000);
        Task<Result<string>> AddDocumentAsync(string? indexName, T document);
        Task<Result<string>> DeleteDocumentAsync(string? indexName, string id);
        Task<bool> CreateIndexAsync(string indexName);
        Task<bool> DeleteIndexAsync(string indexName);
    }
}