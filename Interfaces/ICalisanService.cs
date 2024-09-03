using Demo.Entities;

namespace Demo.Interfaces
{
    public interface ICalisanService
    {
        Task AddAsync(Calisan calisan);
    }
}