using System.Text.RegularExpressions;
using Cw5.DTO_s.Requests;
using Cw5.Services;
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
    }
}