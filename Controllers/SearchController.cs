using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public SearchController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet("get-all-documents")]
        public IActionResult GetAllDocuments(string? indexName, int size = 1000)
        {
            indexName ??= _elasticClient.ConnectionSettings.DefaultIndex;

            var response = _elasticClient.Search<Document>(s => s
                .Index(indexName)
                .Query(q => q.MatchAll())
                .Size(size)
            );

            if (response.IsValid)
            {
                return Ok(response.Documents);
            }
            return BadRequest(response.OriginalException?.Message);
        }

        [HttpPost]
        public IActionResult AddDocument([FromBody] Document document)
        {
            var response = _elasticClient.IndexDocument(document);
            if (response.IsValid)
            {
                return Ok(response.Id);
            }
            return BadRequest(response.OriginalException?.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(string id)
        {
            var response = _elasticClient.Delete<Document>(id);
            if (response.IsValid)
            {
                return Ok();
            }
            return NotFound(response.OriginalException?.Message);
        }

        [HttpPost("create-index")]
        public IActionResult CreateIndex(string indexName)
        {
            var createIndexResponse = _elasticClient.Indices.Create(indexName, c => c
                .Map<Document>(m => m
                    .AutoMap()
                )
            );

            if (createIndexResponse.IsValid)
            {
                return Ok($"Index {indexName} created successfully.");
            }
            return BadRequest(createIndexResponse.OriginalException?.Message);
        }

        [HttpPost("add-to-index/{indexName}")]
        public IActionResult AddDocumentToIndex(string indexName, [FromBody] Document document)
        {
            var response = _elasticClient.Index(document, idx => idx.Index(indexName));
            if (response.IsValid)
            {
                return Ok(response.Id);
            }
            return BadRequest(response.OriginalException?.Message);
        }

        [HttpDelete("delete-from-index/{indexName}/{id}")]
        public IActionResult DeleteDocumentFromIndex(string indexName, string id)
        {
            var response = _elasticClient.Delete(new DeleteRequest(indexName, id));
            if (response.IsValid)
            {
                return Ok();
            }
            return NotFound(response.OriginalException?.Message);
        }

        [HttpDelete("delete-index/{indexName}")]
        public IActionResult DeleteIndex(string indexName)
        {
            var response = _elasticClient.Indices.Delete(indexName);
            if (response.IsValid)
            {
                return Ok($"Index {indexName} deleted successfully.");
            }
            return BadRequest(response.OriginalException?.Message);
        }
    }

    public class Document
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}