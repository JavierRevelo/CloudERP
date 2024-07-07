﻿using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.InventoryMovementDTOs;
using backendfepon.DTOs.InventoryMovementTypeDTO;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    .Include(p => p.Transaction)
                    .Include(p => p.InventoryMovementType)
                    .Include(p => p.Product)
                    .Select(p => new InventoryMovementDTO
                    {
                        Movement_Id = p.Movement_Id,
                        Transaction_Id = p.Transaction_Id,
                        Inventory_Movement_Name = p.InventoryMovementType.Movement_Type_Name,
                        Product_Name = p.Product.Name,
                        Quantity = p.Quantity,
                        Date = p.Date
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
                    .Include(p => p.Transaction)
                    .Include(p => p.InventoryMovementType)
                    .Include(p => p.Product)
                    .Where(p => p.Movement_Id == id)
                    .Select(p => new InventoryMovementDTO
                    {
                        Movement_Id = p.Movement_Id,
                        Transaction_Id = p.Transaction_Id,
                        Inventory_Movement_Name = p.InventoryMovementType.Movement_Type_Name,
                        Product_Name = p.Product.Name,
                        Quantity = p.Quantity,
                        Date = p.Date
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
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == inventoryMovementDTO.Product_Name);
                if (product == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del producto no válido."));
                }

                var inventoryMovementType = await _context.InventoryMovementTypes.FirstOrDefaultAsync(imt => imt.Movement_Type_Name == inventoryMovementDTO.Inventory_Movement_Type_Name);
                if (inventoryMovementType == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del tipo de movimiento de inventario no válido."));
                }

                var inventoryMovement = _mapper.Map<InventoryMovement>(inventoryMovementDTO);
                inventoryMovement.Product_Id = product.Product_Id;
                inventoryMovement.Inventory_Movement_Id = inventoryMovementType.Movement_Type_Id;

                _context.InventoryMovements.Add(inventoryMovement);
                await _context.SaveChangesAsync();

                var createdInventoryMovementDTO = _mapper.Map<InventoryMovementDTO>(inventoryMovement);

                return CreatedAtAction(nameof(GetInventoryMovement), new { id = inventoryMovement.Movement_Id }, createdInventoryMovementDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el movimiento de inventario"));
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

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == updatedInventoryMovement.Product_Name);
                if (product == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del producto no válido."));
                }

                var inventoryMovementType = await _context.InventoryMovementTypes.FirstOrDefaultAsync(imt => imt.Movement_Type_Name == updatedInventoryMovement.Inventory_Movement_Type_Name);
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
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el movimiento de inventario"));
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

        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.Movement_Id == id);
        }
    }
}
