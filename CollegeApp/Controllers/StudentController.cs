using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowAll")]
    [Authorize(AuthenticationSchemes = "LoginForLocalUsers", Roles = "Superadmin,Admin")]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        //private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository;
        private APIResponse _apiResponse;
        public StudentController(ILogger<StudentController> logger,
            IMapper mapper, IStudentRepository studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse = new();
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[EnableCors(PolicyName = "AllowOnlyMicrosoft")]
        //[AllowAnonymous]
        public async Task<ActionResult<APIResponse>> GetStudentsAsync()
        {
            try
            {
                _logger.LogInformation("GetStudents method started");
                var students = await _studentRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //OK - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[DisableCors]
        public async Task<ActionResult<APIResponse>> GetStudentByIdAsync(int id)
        {
            try
            {
                //BadRequest - 400 - Badrequest - Client error
                if (id <= 0)
                {
                    _logger.LogWarning("Bad Request");
                    return BadRequest();
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                //NotFound - 404 - NotFound - Client error
                if (student == null)
                {
                    _logger.LogError("Student not found with given Id");
                    return NotFound($"The student with id {id} not found");
                }

                _apiResponse.Data = _mapper.Map<StudentDTO>(student);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //OK - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentByNameAsync(string name)
        {
            try
            {
                //BadRequest - 400 - Badrequest - Client error
                if (string.IsNullOrEmpty(name))
                    return BadRequest();

                var student = await _studentRepository.GetAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
                //NotFound - 404 - NotFound - Client error
                if (student == null)
                    return NotFound($"The student with name {name} not found");

                _apiResponse.Data = _mapper.Map<StudentDTO>(student);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //OK - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }

        [HttpPost]
        [Route("Create")]
        //api/student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateStudentAsync([FromBody] StudentDTO dto)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                if (dto == null)
                    return BadRequest();

                //if(model.AdmissionDate < DateTime.Now)
                //{
                //    //1. Directly adding error message to modelstate
                //    //2. Using custom attribute
                //    ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
                //    return BadRequest(ModelState);
                //}    

                Student student = _mapper.Map<Student>(dto);

                var studentAfterCreation = await _studentRepository.CreateAsync(student);

                dto.Id = studentAfterCreation.Id;

                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //Status - 201
                //https://localhost:7185/api/Student/3
                //New student details
                return CreatedAtRoute("GetStudentById", new { id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    BadRequest();

                var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);

                if (existingStudent == null)
                    return NotFound();

                var newRecord = _mapper.Map<Student>(dto);

                await _studentRepository.UpdateAsync(newRecord);

                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    BadRequest();

                var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);

                if (existingStudent == null)
                    return NotFound();

                var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

                patchDocument.ApplyTo(studentDTO, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingStudent = _mapper.Map<Student>(studentDTO);

                await _studentRepository.UpdateAsync(existingStudent);

                //204 - NoContent
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }


        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        //api/student/delete/1
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteStudentAsync(int id)
        {
            try
            {
                //BadRequest - 400 - Badrequest - Client error
                if (id <= 0)
                    return BadRequest();

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                //NotFound - 404 - NotFound - Client error
                if (student == null)
                    return NotFound($"The student with id {id} not found");

                await _studentRepository.DeleteAsync(student);
                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //OK - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }
    }
}
