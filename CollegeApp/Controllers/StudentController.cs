using CollegeApp.Data;
using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;
        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("GetStudents method started");
            var students = _dbContext.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Address = s.Address,
                Email = s.Email,
                DOB = s.DOB
            }).ToList();
            //OK - 200 - Success
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"The student with id {id} not found");
            }

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address
            };
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with name {name} not found");
            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address
            };
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        //api/student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (model == null)
                return BadRequest();

            //if(model.AdmissionDate < DateTime.Now)
            //{
            //    //1. Directly adding error message to modelstate
            //    //2. Using custom attribute
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
            //    return BadRequest(ModelState);
            //}    

            Student student = new Student
            {
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();

            model.Id = student.Id;
            //Status - 201
            //https://localhost:7185/api/Student/3
            //New student details
            return CreatedAtRoute("GetStudentById", new { id = model.Id}, model);
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existingStudent = _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;
            existingStudent.DOB = model.DOB;
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existingStudent = _dbContext.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            _dbContext.SaveChanges();

            //204 - NoContent
            return NoContent();
        }


        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        //api/student/delete/1
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
                return BadRequest();

            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with id {id} not found");

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();

            //OK - 200 - Success
            return Ok(true);
        }
    }
}
