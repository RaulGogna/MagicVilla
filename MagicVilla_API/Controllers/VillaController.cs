using AutoMapper;

using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repositories.IRepository;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaController(
            ILogger<VillaController> logger, 
            IVillaRepositorio villaRepo, 
            IMapper mapper, 
            APIResponse response)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo las villas");

                IEnumerable<Villa> villaList = await _villaRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString()};
            }

            return _response;
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>>GetVilla(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer la Villa con Id " + id);
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.GetItem(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.GetItem(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreYaExiste", $"{createDto.Nombre} YA EXISTE!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                    return BadRequest(createDto);

                Villa modelo = _mapper.Map<Villa>(createDto);

                await _villaRepo.Crear(modelo);

                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.GetItem(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }

                await _villaRepo.Remove(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto) 
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.GetItem(v => v.Id == id);

                Villa modelo = _mapper.Map<Villa>(updateDto);

                await _villaRepo.Actualizar(modelo);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            
            return BadRequest(_response);
        }
        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto) 
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.GetItem(v => v.Id == id, tracked: false);

                if (villa == null)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Villa modelo = _mapper.Map<Villa>(villaDto);

                await _villaRepo.Actualizar(modelo);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessed = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_response);
        }
    }
}
