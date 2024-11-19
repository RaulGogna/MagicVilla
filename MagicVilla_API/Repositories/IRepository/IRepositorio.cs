using System.Linq.Expressions;

namespace MagicVilla_API.Repositories.IRepository
{
    public interface IRepositorio<T> where T : class
    {
        Task Crear(T entity);

        Task<T> GetItem(Expression<Func<T, bool>>? filter = null, bool tracked = true);

        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);

        Task Remove(T entity);

        Task Save();
    }
}
