using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api_ofertar.DTOs;
using api_ofertar.Services;
using api_ofertar.Responses;
using api_ofertar.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api_ofertar.Controllers
{
    // Preciso colocar para pegar tither da igreja do usuario logado

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ActiveChurch")]
    public class TitherController : ControllerBase
    {
        private readonly TitherService _titherService;

        public TitherController(TitherService titherService)
        {
            _titherService = titherService;
        }

        private int? GetChurchIdFromJwt()
        {
            var churchIdClaim = User.FindFirst("churchId");
            if (churchIdClaim != null && int.TryParse(churchIdClaim.Value, out var churchId))
            {
                return churchId;
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Tither>>> Create([FromBody] TitherCreateDTO titherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid tither data"));

                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));

                var result = await _titherService.CreateTitherAsync(titherDto, churchId.Value);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<Tither>.Ok(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Tither>>>> GetAll()
        {
            try
            {
                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));
                var tithers = await _titherService.GetAllTithersAsync(churchId.Value);
                return Ok(ApiResponse<List<Tither>>.Ok(tithers, take: tithers.Count, offset: 0, total: tithers.Count));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Tither>>> GetById(int id)
        {
            try
            {
                var tither = await _titherService.GetTitherByIdAsync(id);
                if (tither == null)
                    return NotFound(ApiResponse<object>.Fail("Tither not found"));

                return Ok(ApiResponse<Tither>.Ok(tither));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Tither>>> Update(int id, [FromBody] TitherUpdateDTO titherDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid tither data"));

                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));

                var result = await _titherService.UpdateTitherAsync(id, titherDto, churchId.Value);
                return Ok(ApiResponse<Tither>.Ok(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _titherService.DeleteTitherAsync(id);
                return Ok(ApiResponse<string>.Ok("", message: "Tither deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}
