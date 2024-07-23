using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.ResponsibleDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsibleController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ResponsibleController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Responsible
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibleDTO>>> GetResponsibles()
        {
            try
            {
                var responsibles = await _context.Responsibles
                    .Include(p => p.Event)
                    .Include(p => p.AdministrativeMember)
                    .Select(p => new ResponsibleDTO
                    {
                        Responsible_Id = p.Responsible_Id,
                        AdministrativeMember_Name = p.AdministrativeMember.Name + ' ' + p.AdministrativeMember.Last_Name,
                        Event_Name = p.Event.Title,
                        Event_Role = p.Event_Role
                    })
                    .ToListAsync();

                return Ok(responsibles);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los responsables"));
            }
        }

        // GET: api/Responsible/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsibleDTO>> GetResponsible(int id)
        {
            try
            {
                var responsible = await _context.Responsibles
                    .Include(p => p.Event)
                    .Include(p => p.AdministrativeMember)
                    .Where(p => p.Responsible_Id == id)
                    .Select(p => new ResponsibleDTO
                    {
                        Responsible_Id = p.Responsible_Id,
                        AdministrativeMember_Name = p.AdministrativeMember.Name + ' ' + p.AdministrativeMember.Name,
                        Event_Name = p.Event.Title,
                        Event_Role = p.Event_Role
                    })
                    .FirstOrDefaultAsync();

                if (responsible == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Responsable no encontrado."));
                }

                return Ok(responsible);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el responsable"));
            }
        }

        // POST: api/Responsible
        [HttpPost]
        public async Task<ActionResult<ResponsibleDTO>> PostResponsible(CreateUpdateResponsibleDTO responsibleDTO)
        {
            try
            {
                var administrativeMember = await _context.AdministrativeMembers.FirstOrDefaultAsync(am => am.Email == responsibleDTO.AdministrativeMember_Email);
                if (administrativeMember == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Correo electrónico del miembro administrativo no válido."));
                }

                var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Title == responsibleDTO.Event_Name);
                if (eventEntity == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del evento no válido."));
                }

                var responsible = _mapper.Map<Responsible>(responsibleDTO);
                responsible.Administrative_Member_Id = administrativeMember.Administrative_Member_Id;
                responsible.Event_Id = eventEntity.Event_Id;

                _context.Responsibles.Add(responsible);
                await _context.SaveChangesAsync();

                var createdResponsibleDTO = _mapper.Map<ResponsibleDTO>(responsible);

                return CreatedAtAction(nameof(GetResponsible), new { id = responsible.Responsible_Id }, createdResponsibleDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el responsable."));
            }
        }

        // PUT: api/Responsible/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponsible(int id, CreateUpdateResponsibleDTO updatedResponsible)
        {
            try
            {
                var responsible = await _context.Responsibles.FindAsync(id);
                if (responsible == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID de responsable no válido."));
                }

                var administrativeMember = await _context.AdministrativeMembers.FirstOrDefaultAsync(am => am.Email == updatedResponsible.AdministrativeMember_Email);
                if (administrativeMember == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Correo electrónico del miembro administrativo no válido."));
                }

                var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Title == updatedResponsible.Event_Name);
                if (eventEntity == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del evento no válido."));
                }

                _mapper.Map(updatedResponsible, responsible);
                responsible.Administrative_Member_Id = administrativeMember.Administrative_Member_Id;
                responsible.Event_Id = eventEntity.Event_Id;

                _context.Entry(responsible).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsibleExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Responsable no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el responsable."));
            }
        }

        // DELETE: api/Responsible/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponsible(int id)
        {
            try
            {
                var responsible = await _context.Responsibles.FindAsync(id);
                if (responsible == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Responsable no encontrado."));
                }

                _context.Responsibles.Remove(responsible);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el responsable."));
            }
        }

        private bool ResponsibleExists(int id)
        {
            return _context.Responsibles.Any(e => e.Responsible_Id == id);
        }
    }
}
