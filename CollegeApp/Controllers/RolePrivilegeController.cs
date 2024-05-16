using AutoMapper;
using CollegeApp.Data.Repository;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        private APIResponse _apiResponse;
        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _apiResponse = new();
        }
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRolePrivilegeAsync(RolePrivilegeDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();

                RolePrivilege rolePrivilege = _mapper.Map<RolePrivilege>(dto);
                rolePrivilege.IsDeleted = false;
                rolePrivilege.CreatedDate = DateTime.Now;
                rolePrivilege.ModifiedDate = DateTime.Now;

                var result = await _rolePrivilegeRepository.CreateAsync(rolePrivilege);

                dto.Id = result.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                //return Ok(_apiResponse);
                return CreatedAtRoute("GetRolePrivilegeById", new {id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("All", Name = "GetAllRolePrivileges")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync()
        {
            try
            {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("AllRolePrivilegesByRoleId", Name = "GetAllRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesByRoleIdAsync(int roleId)
        {
            try
            {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllByFilterAsync(rolePrivilege => rolePrivilege.RoleId == roleId);

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(rolePrivilege => rolePrivilege.Id == id);

                if (rolePrivilege == null)
                    return NotFound($"The Role Privilege not found with id: {id}");

                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest();

                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(role => role.RolePrivilegeName.Contains(name));

                if (rolePrivilege == null)
                    return NotFound($"The Role Privilege not found with name: {name}");

                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilege(RolePrivilegeDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return BadRequest();

                var existingRolePrivilege = await _rolePrivilegeRepository.GetAsync(role => role.Id == dto.Id, true);

                if (existingRolePrivilege == null)
                    return BadRequest($"Role Privilege not found with id: {dto.Id} to update");

                var newRolePrivilege = _mapper.Map<RolePrivilege>(dto);

                await _rolePrivilegeRepository.UpdateAsync(newRolePrivilege);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = newRolePrivilege;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpDelete]
        [Route("Delete/{id}", Name = "DeleteRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRolePrivilegeAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(rolePrivilege => rolePrivilege.Id == id);

                if (rolePrivilege == null)
                    return BadRequest($"Role Privilege not found with id: {id} to delete");

                await _rolePrivilegeRepository.DeleteAsync(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = true;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
