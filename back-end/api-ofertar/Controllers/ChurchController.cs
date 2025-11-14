using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api_ofertar.DTOs;
using api_ofertar.Services;
using api_ofertar.Responses;
using Microsoft.AspNetCore.Authorization;
using api_ofertar.Entities;

namespace api_ofertar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChurchController : ControllerBase
    {
        private readonly ChurchService _churchService;

        public ChurchController(ChurchService churchService)
        {
            _churchService = churchService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Church>>>> GetAll()
        {
            try
            {
                var churches = await _churchService.GetAllChurchesAsync();
                return Ok(ApiResponse<List<Church>>.Ok(churches, take: churches.Count, offset: 0, total: churches.Count));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Church>>> GetById(int id)
        {
            try
            {
                var church = await _churchService.GetChurchByIdAsync(id);
                return Ok(ApiResponse<Church>.Ok(church));
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

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<Church>>> Create([FromBody] ChurchCreateDTO churchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid church data"));

                var result = await _churchService.CreateChurchAsync(churchDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<Church>.Ok(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<Church>>> Update(int id, [FromBody] ChurchUpdateDTO churchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid church data"));

                var result = await _churchService.UpdateChurchAsync(id, churchDto);
                return Ok(ApiResponse<Church>.Ok(result));
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _churchService.DeleteChurchAsync(id);
                return Ok(ApiResponse<string>.Ok("", message: "Church deleted successfully"));
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
