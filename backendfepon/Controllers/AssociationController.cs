using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AssociationController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Association
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociationDTO>>> GetAssociations()
        {
            try
            {
                var associations = await _context.Associations
                    .Include(p => p.State)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new AssociationDTO
                    {
                        id = p.Association_Id,
                        state_id = p.State_Id,
                        mission = p.Mission,
                        vision = p.Vision,
                    })
                    .ToListAsync();

                return Ok(associations);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la Asociación"));
            }
        }

        // GET: api/Association/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssociationDTO>> GetAssociation(int id)
        {
            try
            {
                var association = await _context.Associations
                    .Include(p => p.State)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE && p.Association_Id == id)
                    .Select(p => new AssociationDTO
                    {
                        id = p.Association_Id,
                        state_id = p.State_Id,

                        mission = p.Mission,
                        vision = p.Vision,
                    })
                    .FirstOrDefaultAsync();

                if (association == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Asociación no encontrada."));
                }

                return Ok(association);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la Asociación"));
            }
        }

        // POST: api/Association
        [HttpPost]
        public async Task<ActionResult<AssociationDTO>> PostAssociation(CreateUpdateAssociationDTO associationDTO)
        {
            try
            {

                var association = _mapper.Map<Association>(associationDTO);
                association.State_Id = Constants.DEFAULT_STATE;

                _context.Associations.Add(association);
                await _context.SaveChangesAsync();

                var createdAssociationDTO = _mapper.Map<AssociationDTO>(association);

                return CreatedAtAction(nameof(GetAssociation), new { id = association.Association_Id }, createdAssociationDTO);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error al actualizar la base de datos, no es posible crear la Asociación", ex));
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle AutoMapper exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error en la configuración del mapeo, no es posible crear la Asociación", ex));
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear la Asociación", ex));
            }
        }

        // PUT: api/Association/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssociation(int id, CreateUpdateAssociationDTO updatedAssociation)
        {
            try
            {
                var association = await _context.Associations.FindAsync(id);
                if (association == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID de la asociación no válido."));
                }

                _mapper.Map(updatedAssociation, association);

                _context.Entry(association).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociationExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Asociación no encontrada."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar la Asociación"));
            }
        }

        // PATCH: api/Products/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProductState(int id)
        {
            try
            {
                var association = await _context.Associations.FindAsync(id);
                if (association == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Asociacion no encontrada."));
                }

                association.State_Id = Constants.STATE_INACTIVE;
                _context.Entry(association).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociationExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Asociacion no encontrada."));
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
        
        private bool AssociationExists(int id)
        {
            return _context.Associations.Any(e => e.Association_Id == id);
        }
    }
}
