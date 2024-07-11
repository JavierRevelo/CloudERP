using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.InventoryMovementDTOs;
using backendfepon.DTOs.InventoryMovementTypeDTO;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryMovementController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InventoryMovementController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/InventoryMovement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovementDTO>>> GetInventoryMovements()
        {
            try
            {
                var inventoryMovements = await _context.InventoryMovements
                    .Include(p => p.InventoryMovementType)
                    .Include(p => p.Product)
                    .Include(p => p.State)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new InventoryMovementDTO
                    {
                        id = p.Movement_Id,
                        stateid = p.State_Id,
                        movementType = p.InventoryMovementType.Movement_Type_Name,
                        product = p.Product.Name,
                        quantity = p.Quantity,
                        date = p.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .ToListAsync();

                return Ok(inventoryMovements);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el movimiento de inventario"));
            }
        }

        // GET: api/InventoryMovement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovementDTO>> GetInventoryMovement(int id)
        {
            try
            {
                var inventoryMovement = await _context.InventoryMovements
                    .Include(p => p.InventoryMovementType)
                    .Include(p => p.Product)
                    .Include(p => p.State)
                    .Where(p => p.Movement_Id == id)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new InventoryMovementDTO
                    {
                        id = p.Movement_Id,
                        stateid = p.State_Id,
                        movementType = p.InventoryMovementType.Movement_Type_Name,
                        product = p.Product.Name,
                        quantity = p.Quantity,
                        date = p.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .FirstOrDefaultAsync();

                if (inventoryMovement == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Movimiento de inventario no encontrado."));
                }

                return Ok(inventoryMovement);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el movimiento de inventario"));
            }
        }

        // POST: api/InventoryMovement
        [HttpPost]
        public async Task<ActionResult<InventoryMovementDTO>> PostInventoryMovement(CreateUpdateInventoryMovementDTO inventoryMovementDTO)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == inventoryMovementDTO.product_Name);
                if (product == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del producto no válido."));
                }

                var inventoryMovementType = await _context.InventoryMovementTypes.FirstOrDefaultAsync(imt => imt.Movement_Type_Name == inventoryMovementDTO.inventory_Movement_Type_Name);
                if (inventoryMovementType == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del tipo de movimiento de inventario no válido."));
                }

                var inventoryMovement = _mapper.Map<InventoryMovement>(inventoryMovementDTO);
                inventoryMovement.Product_Id = product.Product_Id;
                inventoryMovement.Inventory_Movement_Id = inventoryMovementType.Movement_Type_Id;
                inventoryMovement.State_Id = Constants.DEFAULT_STATE;

                _context.InventoryMovements.Add(inventoryMovement);
                await _context.SaveChangesAsync();

                var createdInventoryMovementDTO = _mapper.Map<InventoryMovementDTO>(inventoryMovement);

                return CreatedAtAction(nameof(GetInventoryMovement), new { id = inventoryMovement.Movement_Id }, createdInventoryMovementDTO);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error al actualizar la base de datos, no es posible crear el movimiento de inventario", ex));
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle AutoMapper exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error en la configuración del mapeo, no es posible crear el movimiento de inventario", ex));
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el movimiento de inventario", ex));
            }
          
        }

        // PUT: api/InventoryMovement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryMovement(int id, CreateUpdateInventoryMovementDTO updatedInventoryMovement)
        {
            try
            {
                var inventoryMovement = await _context.InventoryMovements.FindAsync(id);
                if (inventoryMovement == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID del movimiento de inventario no válido."));
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == updatedInventoryMovement.product_Name);
                if (product == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del producto no válido."));
                }

                var inventoryMovementType = await _context.InventoryMovementTypes.FirstOrDefaultAsync(imt => imt.Movement_Type_Name == updatedInventoryMovement.inventory_Movement_Type_Name);
                if (inventoryMovementType == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del tipo de movimiento de inventario no válido."));
                }

                _mapper.Map(updatedInventoryMovement, inventoryMovement);
                inventoryMovement.Product_Id = product.Product_Id;
                inventoryMovement.Inventory_Movement_Id = inventoryMovementType.Movement_Type_Id;

                _context.Entry(inventoryMovement).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryMovementExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Movimiento de inventario no encontrado."));
                    }
                    else
                    {
                        return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error de concurrencia."));
                    }
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error al actualizar la base de datos, no es posible crear el movimiento de inventario", ex));
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle AutoMapper exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error en la configuración del mapeo, no es posible crear el movimiento de inventario", ex));
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el movimiento de inventario", ex));
            }
        }

        // DELETE: api/InventoryMovement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryMovement(int id)
        {
            try
            {
                var inventoryMovement = await _context.InventoryMovements.FindAsync(id);
                if (inventoryMovement == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Movimiento de inventario no encontrado."));
                }

                _context.InventoryMovements.Remove(inventoryMovement);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el movimiento de inventario"));
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchInventoryMovementState(int id)
        {
            try
            {
                var inventoryMovement = await _context.InventoryMovements.FindAsync(id);
                if (inventoryMovement == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Movimiento de inventario no encontrado."));
                }

                inventoryMovement.State_Id = Constants.STATE_INACTIVE;
                _context.Entry(inventoryMovement).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryMovementExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Movimiento de inventario no encontrado."));
                    }
                    else
                    {
                        return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error de concurrencia."));
                    }
                }

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el estado"));
            }
        }


        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.Movement_Id == id);
        }
    }
}
