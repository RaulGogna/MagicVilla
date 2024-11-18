using MagicVilla_API.Models;

using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la villa...",
                    ImageUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 5,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualización = DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa Real Luxury",
                    Detalle = "Best Luxury Villa",
                    ImageUrl = "",
                    Ocupantes = 10,
                    MetrosCuadrados = 20,
                    Tarifa = 400,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualización = DateTime.Now,
                }
            );
        }
    }
}
