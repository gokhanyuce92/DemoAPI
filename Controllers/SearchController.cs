using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService<Document> _searchService;
        public SearchController(ISearchService<Document> searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("get-all-documents")]
        public async Task<IActionResult> GetAllDocuments(string? indexName)
        {
            var response = await _searchService.GetAllDocumentsAsync(indexName);

            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.ErrorMessage);
        }

        [HttpPost("create-index")]
        public async Task<IActionResult> CreateIndex(string indexName)
        {
            var createIndexResponse = await _searchService.CreateIndexAsync(indexName);

            if (createIndexResponse)
            {
                return Ok("Index created successfully");
            }
            return BadRequest("Failed to create index");
        }

        [HttpDelete("delete-index/{indexName}")]
        public async Task<IActionResult> DeleteIndex(string indexName)
        {
            var deleteIndexResponse = await _searchService.DeleteIndexAsync(indexName);

            if (deleteIndexResponse)
            {
                return Ok("Index deleted successfully.");
            }
            return NotFound("Index not found.");
        }

        [HttpPost("add-document")]
        public async Task<IActionResult> AddDocument(string? indexName, [FromBody] Document document)
        {
            var response = await _searchService.AddDocumentAsync(indexName, document);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.ErrorMessage);
        }

        [HttpDelete("delete-document/{id}")]
        public async Task<IActionResult> DeleteDocument(string? indexName, string id)
        {
            var response = await _searchService.DeleteDocumentAsync(indexName, id);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound(response.ErrorMessage);
        }
    }
}