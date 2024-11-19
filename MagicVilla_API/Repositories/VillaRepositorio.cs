using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepository;

namespace MagicVilla_API.Repositories
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public VillaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualización = DateTime.Now;
            _db.Villas.Update(entidad);

            await Save();
            return entidad;
        }
    }
}
