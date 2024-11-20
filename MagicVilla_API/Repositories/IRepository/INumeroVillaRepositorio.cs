using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories.IRepository
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
