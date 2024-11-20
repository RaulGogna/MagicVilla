using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepository;

namespace MagicVilla_API.Repositories
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public NumeroVillaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVillas.Update(entidad);

            await Save();
            return entidad;
        }
    }
}
