﻿using AutoMapper;

using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repositories.IRepository;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroVillaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(
            ILogger<NumeroVillaController> logger, 
            IVillaRepositorio villaRepo, 
            INumeroVillaRepositorio numeroVillaRepo, 
            IMapper mapper, 
            APIResponse response)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroVillaRepo = numeroVillaRepo;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo el numero de las villas");

                IEnumerable<NumeroVilla> villaList = await _numeroVillaRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<NumeroVillaDto>>(villaList);
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

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>>GetNumeroVilla(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer el numero de la Villa con Id " + id);
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroVillaRepo.GetItem(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<NumeroVillaDto>(numeroVilla);
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
        public async Task<ActionResult<APIResponse>> CrearnNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroVillaRepo.GetItem(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreYaExiste", $"{createDto.VillaNo} YA EXISTE!");
                    return BadRequest(ModelState);
                }

                if(await _villaRepo.GetItem(v => v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForánea", $"El ID {createDto.VillaId} de la villa no existe!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                    return BadRequest(createDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroVillaRepo.Crear(modelo);

                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { modelo.VillaNo }, _response);
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
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroVillaRepo.GetItem(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }

                await _numeroVillaRepo.Remove(numeroVilla);

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
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto) 
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _response.IsSuccessed = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if(await _villaRepo.GetItem(v => v.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForánea", $"El ID {updateDto.VillaId} de la villa no existe!");
                    return BadRequest(ModelState);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

                await _numeroVillaRepo.Actualizar(modelo);

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
