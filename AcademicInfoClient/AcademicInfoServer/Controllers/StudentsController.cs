using AcademicInfoServer.Models;
using AcademicInfoServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcademicInfoServer.Controllers
{
    [ApiController]
    [Route("Students")]
    public class StudentsController : ControllerBase
    {
        StudentRepository _studentRepository;

        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger, StudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        [HttpGet(Name = "List")]
        public IEnumerable<Student> Get()
        {
            Console.Write("entered");
            return _studentRepository.GetStudents();
        }
    }
}
