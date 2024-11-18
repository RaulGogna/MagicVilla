using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obteniendo las villas");
            return Ok(_dbContext.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto>GetVilla(int id) 
        {
            if(id == 0)
            {
                _logger.LogError("Error al traer la Villa con Id " + id);
                return BadRequest();
            }

            var villa = _dbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
                return NotFound();

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(_dbContext.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreYaExiste", $"{villaDto.Nombre} YA EXISTE!");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
                return BadRequest(villaDto);

            if (villaDto.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            Villa modelo = RellenarModelo(villaDto);

            _dbContext.Villas.Add(modelo);
            _dbContext.SaveChanges();

            return CreatedAtRoute("GetVilla", new { villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
                return BadRequest();

            var villa = _dbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null) 
                return NotFound();

            _dbContext.Villas.Remove(villa);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto) 
        {
            if (villaDto == null || id != villaDto.Id)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = RellenarModelo(villaDto);
            _dbContext.Villas.Update(modelo);
            _dbContext.SaveChanges();

            return NoContent();
        }
        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto) 
        {
            if (patchDto == null || id == 0)
                return BadRequest();

            var villa = _dbContext.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            if (villa == null) 
                return NotFound();

            VillaDto villaDto = RellenarModelo(villa);
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Villa modelo = RellenarModelo(villaDto);
            _dbContext.Villas.Update(modelo);
            _dbContext.SaveChanges();

            return NoContent();
        }


        private Villa RellenarModelo(VillaDto villaDto)
        {
            Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                Amenidad = villaDto.Amenidad,
                MetrosCuadrados = villaDto.MetrosCuadrados
            };

            return modelo;
        }

        private VillaDto RellenarModelo(Villa villaDto)
        {
            VillaDto modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                Amenidad = villaDto.Amenidad,
                MetrosCuadrados = villaDto.MetrosCuadrados
            };

            return modelo;
        }
    }
}
