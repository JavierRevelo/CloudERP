using backendfepon.Data;
using backendfepon.DTOs.InventoryMovementTypeDTO;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryMovementTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public InventoryMovementTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InventoryMovementType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovementTypeDTO>>> GetInventoryMovementTypes()
        {
            try
            {
                var inventoryMovementTypes = await _context.InventoryMovementTypes
                    .Select(p => new InventoryMovementTypeDTO
                    {
                        movement_Type_Name = p.Movement_Type_Name,
                    })
                    .ToListAsync();

                return Ok(inventoryMovementTypes);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el movimiento de inventario"));
            }
        }

        // GET: api/InventoryMovementType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovementTypeDTO>> GetInventoryMovementType(int id)
        {
            try
            {
                var inventoryMovementType = await _context.InventoryMovementTypes
                    .Where(p => p.Movement_Type_Id == id)
                    .Select(p => new InventoryMovementTypeDTO
                    {
                        movement_Type_Name = p.Movement_Type_Name,
                    })
                    .FirstOrDefaultAsync();

                if (inventoryMovementType == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Tipo de movimiento de inventario no encontrado."));
                }

                return Ok(inventoryMovementType);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el movimiento de inventario"));
            }
        }

        // POST: api/InventoryMovementType
        [HttpPost]
        public async Task<ActionResult<InventoryMovementTypeDTO>> PostInventoryMovementType(CreateUpdateInventoryMovementTypeDTO inventoryMovementTypeDTO)
        {
            try
            {
                var inventoryMovementType = new InventoryMovementType
                {
                    Movement_Type_Name = inventoryMovementTypeDTO.Movement_Type_Name,
                };

                _context.InventoryMovementTypes.Add(inventoryMovementType);
                await _context.SaveChangesAsync();

                var createdInventoryMovementTypeDTO = new InventoryMovementTypeDTO
                {
                    movement_Type_Name = inventoryMovementType.Movement_Type_Name
                };

                return CreatedAtAction(nameof(GetInventoryMovementType), new { id = inventoryMovementType.Movement_Type_Id }, createdInventoryMovementTypeDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el movimiento de inventario"));
            }
        }

        // DELETE: api/InventoryMovementType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryMovementType(int id)
        {
            try
            {
                var inventoryMovementType = await _context.InventoryMovementTypes.FindAsync(id);

                if (inventoryMovementType == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Tipo de movimiento de inventario no encontrado."));
                }

                _context.InventoryMovementTypes.Remove(inventoryMovementType);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el movimiento de inventario"));
            }
        }
    }
}
