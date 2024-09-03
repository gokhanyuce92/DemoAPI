using Demo.Entities;
using Demo.Interfaces;
using Demo.Repositories.Abstract;

namespace Demo.Services {
    public class CalisanService : ICalisanService
    {
        private readonly ICalisanRepository _calisanRepository;
        public CalisanService(ICalisanRepository calisanRepository)
        {
            _calisanRepository = calisanRepository;
        }
        public async Task AddAsync(Calisan calisan)
        {
            await _calisanRepository.AddAsync(calisan);
        }
    }
}