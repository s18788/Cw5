using System.Text.RegularExpressions;
using Cw5.DTO_s.Requests;
using Cw5.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        private static readonly Regex r = new Regex("^s[0-9]+$");


        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [Route("api/enrollments")]
        [HttpPost]
        [Authorize(Roles= "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            if (request.IndexNumber == null
                || string.IsNullOrEmpty(request.FirstName)
                || string.IsNullOrEmpty(request.LastName)
                || !r.IsMatch(request.IndexNumber)
                || string.IsNullOrEmpty(request.Studies))
            {
                return BadRequest();
            }
            try
            {
                _service.EnrollStudent(request);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }


        [Route("api/enrollments/promotions")]
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            try
            {
                _service.PromoteStudents(request.semester, request.studies);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }

        [Route("api/login")]
        [HttpPost]
        public IActionResult LogIn(LoginRequest request){
        
         var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, request.IndexNumber),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Cw_07",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken=Guid.NewGuid()
            });
        }
                      
    }
}