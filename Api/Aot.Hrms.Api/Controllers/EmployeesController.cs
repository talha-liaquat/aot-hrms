using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpPost()]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody]RegisterEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newEmploee = await _employeeService.RegisterAsync(request);

            return Ok(newEmploee);
        }

        [HttpPost("{employeeId}/verify")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAsync([Required]string employeeId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var updatedEmploee = await _employeeService.VerifyAsync(new VerifyEmployeeRequest { EmployeeId = employeeId });

            return Ok(updatedEmploee);
        }

        [HttpPost("{employeeId}/skills")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateEmployeeSkillsBulkAsync([Required]string employeeId, [FromBody][Required]EmployeeSkillsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var id = await _employeeService.AssignSkillAsync(employeeId, request.SkillIds, employeeId);

            return Ok(id);
        }

        [HttpGet("{employeeId}/skills")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetEmployeeSkillsAsync([Required]string employeeId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var skills = _employeeService.GetSkillsByEmployeeId(employeeId);

            return Ok(skills);
        }

        [HttpGet("skills/{skillId}")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetSkillEmployeesAsync([Required]string skillId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var skills = _employeeService.GetEmployeeBySkillId(skillId);

            return Ok(skills);
        }
    }
}