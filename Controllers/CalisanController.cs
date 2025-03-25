using Microsoft.AspNetCore.Mvc;
using Demo.Entities;
using Demo.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalisanController : ControllerBase
    {
        private readonly ICalisanRepository _calisanRepository;
        public CalisanController(ICalisanRepository calisanRepository)
        {
            _calisanRepository = calisanRepository;
        }

        [HttpGet] 
        public async Task<IActionResult> Get()
        {
            await _calisanRepository.AddAsync(new Calisan
            {
                Ad = "Ali",
                Soyad = "Veli",
                Departman = "Bilgi İşlem",
                Maas = "5000"
            });
            return Ok();
        }

    }
}