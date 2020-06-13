using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aot.Hrms.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        public LookupsController(ILookupService lookupService)
        {
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        [HttpPost("skills")]
        [ProducesResponseType(typeof(RegisterSkillRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSkillAsync([FromBody]RegisterSkillRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newSkill = await _lookupService.CreateSkillAsync(request);

            return Ok(newSkill);
        }

        [HttpGet("skills")]
        [ProducesResponseType(typeof(RegisterSkillRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSkillseAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var skills = _lookupService.GetAllSkills();

            return Ok(skills);
        }
    }
}