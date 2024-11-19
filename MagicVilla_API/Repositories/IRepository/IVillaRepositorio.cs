using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories.IRepository
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
