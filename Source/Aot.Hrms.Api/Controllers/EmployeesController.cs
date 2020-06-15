using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Aot.Hrms.Contracts;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Aot.Hrms.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public EmployeesController(IEmployeeService employeeService, IConfiguration configuration, IEmailService emailService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost()]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody]RegisterEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newEmploee = await _employeeService.RegisterAsync(request);

            var stateObj = new EmailVerificationDto
            {
                Email = newEmploee.Email,
                EmployeeId = newEmploee.Id,
                ValidUntil = DateTime.Now.AddMinutes(15)
            };

            var state = CryptoHelper.Encode(CryptoHelper.Encrypt(CryptoHelper.Serialize(stateObj), _configuration["SecurityConfiguraiton:EncryptionKey"]));

            _emailService.SendEmail(new EmailDto
            {
                To = new List<string> { newEmploee.Email },
                Subject = "AOT Invitation Email Verification",
                IsHtml = true,
                Body = $"Hi {newEmploee.Name},<br/><br/>Welcome to AOT.<br/><br />Please click on below link to verify your email address. Token validity: {stateObj.ValidUntil.ToShortTimeString()}.<br/><br/><a href=\"https://localhost:44300/api/v1/Employees/verify?state={state}\">Please click here to verify</a><br/><br/>Thanks, AOT Team"
            });

            return Ok(newEmploee);
        }

        [HttpGet("verify")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAsync([FromQuery]string state)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var stateObj = CryptoHelper.Deserialize<EmailVerificationDto>(CryptoHelper.Decrypt(CryptoHelper.Decode(state), _configuration["SecurityConfiguraiton:EncryptionKey"]));

            if (stateObj.ValidUntil < DateTime.UtcNow)
                return BadRequest("Token has been expired. Please contact System Administrator!");

            var updatedEmploee = await _employeeService.VerifyAsync(new VerifyEmployeeRequest { EmployeeId = stateObj.EmployeeId });

            stateObj.ValidUntil.AddDays(15);

            var newState = CryptoHelper.Encode(CryptoHelper.Encrypt(CryptoHelper.Serialize(stateObj), _configuration["SecurityConfiguraiton:EncryptionKey"]));

            return RedirectPermanent($"http://localhost:4200/register?email={updatedEmploee.Email}&name={updatedEmploee.Name}&state={newState}");
        }



        [HttpPost("{employeeId}/skills")]
        [ProducesResponseType(typeof(RegisterEmployeeRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetSkillEmployeesAsync([Required]string skillId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var skills = _employeeService.GetEmployeeBySkillId(skillId);

            return Ok(skills);
        }
    }
}