using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _repository.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found" });
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra RoleName đã tồn tại
            var existingRole = await _repository.GetByNameAsync(role.RoleName);
            if (existingRole != null)
            {
                return BadRequest(new { Message = "Role name already exists" });
            }

            await _repository.AddAsync(role);
            return CreatedAtAction(nameof(GetRole), new { id = role.RoleId }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest(new { Message = "Role ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRole = await _repository.GetByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound(new { Message = "Role not found" });
            }

            // Kiểm tra RoleName đã tồn tại (trừ chính role đang sửa)
            var duplicateRole = await _repository.GetByNameAsync(role.RoleName);
            if (duplicateRole != null && duplicateRole.RoleId != id)
            {
                return BadRequest(new { Message = "Role name already exists" });
            }

            existingRole.RoleName = role.RoleName;
            await _repository.UpdateAsync(existingRole);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found" });
            }

            // Kiểm tra xem vai trò có đang được sử dụng bởi người dùng
            if (role.Users.Any())
            {
                return BadRequest(new { Message = "Cannot delete role because it is assigned to users" });
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
