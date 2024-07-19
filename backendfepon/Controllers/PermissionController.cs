using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.EventDTOs;
using backendfepon.DTOs.EventExpenseDTO;
using backendfepon.DTOs.PermissionDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PermissionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Permission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetPermissions()
        {
            try
            {
                var permissions = await _context.Permissions
                    // .Include(p => p.Event)
                    .Select(p => new PermissionDTO
                    {
                        Permission_Id = p.Permission_Id,
                        // Event_Name = p.Event.Title,
                        Request = p.Request,
                        Request_Status = p.FinancialRequestState.State_Description
                    })
                    .ToListAsync();

                return Ok(permissions);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el permiso"));
            }
        }

        // GET: api/Permission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermission(int id)
        {
            try
            {
                var permission = await _context.Permissions
                    //.Include(p => p.Event)
                    .Where(p => p.Permission_Id == id)
                    .Select(p => new PermissionDTO
                    {
                        Permission_Id = p.Permission_Id,
                        // Event_Name = p.Event.Title,
                        Request = p.Request,
                        Request_Status = p.FinancialRequestState.State_Description
                    })
                    .FirstOrDefaultAsync();

                if (permission == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Permiso no encontrado."));
                }

                return Ok(permission);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el permiso"));
            }
        }

        // POST: api/Permission
        [HttpPost]
        public async Task<ActionResult<PermissionDTO>> PostPermission(CreateUpdatePermissionDTO permissionDTO)
        {
            try
            {
                var permission = _mapper.Map<Permission>(permissionDTO);

                // Suponiendo que tienes lógica para resolver el Status_Id basado en el Request_Status proporcionado
                var status = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == permissionDTO.Request_Status);
                if (status == null)
                {
                    return BadRequest("Estado de solicitud no válido.");
                }
                permission.Status_Id = status.Request_State_Id;

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();

                var createdPermissionDTO = _mapper.Map<PermissionDTO>(permission);
                return CreatedAtAction(nameof(GetPermission), new { id = permission.Permission_Id }, createdPermissionDTO);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error interno del servidor, no es posible crear el permiso");
            }
        }

        [HttpPut("/updatePermission/{id}")]
        public async Task<IActionResult> UpdateEventPermission(int id, CreateUpdatePermissionDTO permissionDTO)
        {
            try
            {
                var status = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == permissionDTO.Request_Status);
                // Crear un nuevo objeto Permission con los datos del DTO
                var newPermission = new Permission
                {
                    Status_Id = status.Request_State_Id,
                    Request = permissionDTO.Request,
                    // Asignar otros campos según sea necesario
                };

                // Agregar el nuevo Permission a la base de datos
                _context.Permissions.Add(newPermission);
                await _context.SaveChangesAsync();

                // Obtener el ID del nuevo Permission generado
                var newPermissionId = await _context.Permissions.FirstOrDefaultAsync(s => s.Request == permissionDTO.Request);

                // Buscar el evento que se va a actualizar
                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID del evento no válido."));
                }

                // Asignar el nuevo Permission_Id al evento existente
                existingEvent.Permission_Id = newPermissionId.Permission_Id;

                // Guardar los cambios en el evento
                _context.Entry(existingEvent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el evento con nuevo permiso", ex));
            }
        }




        // DELETE: api/Permission/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);
                if (permission == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Permiso no encontrado."));
                }

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el permiso"));
            }
        }

        private bool PermissionExists(int id)
        {
            return _context.Permissions.Any(e => e.Permission_Id == id);
        }
    }
}
