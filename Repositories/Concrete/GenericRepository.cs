using Demo.Models;
using Demo.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyDbContext _context;
        public GenericRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Result<T>> AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return new Result<T> { IsSuccess = true, Data = entity };
            }
            catch (Exception ex)
            {
                // Örneğin: _logger.LogError(ex, "Varlık eklenirken hata oluştu."); 
                return new Result<T> { IsSuccess = false, ErrorMessage = "Varlık eklenirken hata oluştu." + ex.Message };
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}