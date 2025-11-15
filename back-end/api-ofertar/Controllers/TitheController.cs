using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.DTOs;
using api_ofertar.Entities;
using api_ofertar.Responses;
using api_ofertar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_ofertar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ActiveChurch")]
    public class TitheController : ControllerBase
    {
       private readonly TitheService _titheService;

        public TitheController(TitheService titheService)
        {
            _titheService = titheService;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Tithe>>> GetById(int id)
        {
            try
            {
                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));
                var tithe = await _titheService.GetTitheByIdAsync(id, churchId.Value);
                return Ok(ApiResponse<Tithe>.Ok(tithe));
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Tithe>>>> GetAll(){
            try
            {
                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));

                var tithes = await _titheService.GetAllTithesAsync(churchId.Value);
                return Ok(ApiResponse<List<Tithe>>.Ok(tithes, take: tithes.Count, offset: 0, total: tithes.Count));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create(TitheCreateDTO titheDto){
            try
            {
                await _titheService.CreateTitheAsync(titheDto);
                return Ok(ApiResponse<object>.Ok(null, "Tithe created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, TitheUpdateDTO titheDto){
            try
            {
                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));

                await _titheService.UpdateTitheAsync(id, titheDto, churchId.Value);
                return Ok(ApiResponse<object>.Ok(null, "Tithe updated successfully"));
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
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var churchId = GetChurchIdFromJwt();
                if (!churchId.HasValue)
                    return BadRequest(ApiResponse<object>.Fail("Church ID not found in JWT"));
                await _titheService.DeleteTitheAsync(id, churchId.Value);
                return Ok(ApiResponse<object>.Ok(null, "Tithe deleted successfully"));
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