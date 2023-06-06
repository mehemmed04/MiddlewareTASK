using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApiDemoG.Dtos;
using WebApiDemoG.Services.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiDemoG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public AccountController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: api/<AccountController>
        [HttpPost("SignIn")]
        public IActionResult SignIn(SignInDto dto)
        {
            var student = _studentService.GetAll().FirstOrDefault(s => s.Username == dto.Username && s.Password == dto.Password);
            if (student != null)
            {
                var token = student.Username + ":" + student.Password;
                
                return Ok(Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));
            }
            return Unauthorized();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
