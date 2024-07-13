using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.DTOs.EventDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EventController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            try
            {
                var events = await _context.Events
                    .Include(p => p.State)
                    .Select(p => new EventDTO
                    {
                        id = p.Event_Id,
                        title = p.Title,
                        status= p.State.Event_State_Name,
                        description = p.Description,
                        startDate = p.Start_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        endDate = p.End_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        budget = p.Financial_Request.Value,
                        budgetStatus = p.Financial_Request.Financial_Request_State.State_Description,
                        location = p.Event_Location,
                        income = p.Income.ToString()
                        //Hiring = p.Hiring
                    })
                    .ToListAsync();

                return Ok(events);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el evento"));
            }
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            try
            {
                var singleEvent = await _context.Events
                    .Include(p => p.State)
                    .Where(p => p.State_Id == id)
                    .Select(p => new EventDTO
                    {
                        id = p.Event_Id,
                        title = p.Title,
                        status = p.State.Event_State_Name,
                        description = p.Description,
                        startDate = p.Start_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        endDate = p.End_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        budget = p.Financial_Request.Value,
                        budgetStatus = p.Financial_Request.Financial_Request_State.State_Description,
                        location = p.Event_Location,
                        income = p.Income.ToString()
                    })
                    .FirstOrDefaultAsync();

                if (singleEvent == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Evento no encontrado."));
                }

                return Ok(singleEvent);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el evento"));
            }
        }
        /*
        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(CreateUpdateEventDTO eventDTO)
        {
            try
            {
                var state = await _context.EventStates.FirstOrDefaultAsync(s => s.Event_State_Name == eventDTO.state);
                if (state == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }

                var newEvent = _mapper.Map<Event>(eventDTO);
                newEvent.State_Id = state.Event_State_Id;


                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                var createdEventDTO = _mapper.Map<EventDTO>(newEvent);

                return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Event_Id }, createdEventDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el evento"));
            }
        }
        */
        /*
        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(CreateUpdateEventDTO eventDTO)
        {
            try
            {
                var eventState = await _context.EventStates.FirstOrDefaultAsync(s => s.Event_State_Name == eventDTO.status);
                if (eventState == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }
                var bugetStatus = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == eventDTO.budgetStatus);
                if (eventState == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del budget estado no válido."));
                }

                var newFinancialRequest = new FinancialRequest {
                    Request_Status_Id = bugetStatus.Request_State_Id,
                    Reason = "",
                    Value= eventDTO.budget
                };
                var newEvent = _mapper.Map<Event>(eventDTO);
                newEvent.State_Id = eventState.Event_State_Id;

                _context.FinancialRequests.Add(newFinancialRequest);
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                var createdEventDTO = _mapper.Map<EventDTO>(newEvent);

                return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Event_Id }, createdEventDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el evento"));
            }
        }*/

        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(CreateUpdateEventDTO eventDTO)
        {
            try
            {
                var eventState = await _context.EventStates.FirstOrDefaultAsync(s => s.Event_State_Name == eventDTO.status);
                if (eventState == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }

                var budgetStatus = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == eventDTO.budgetStatus);
                if (budgetStatus == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado de presupuesto no válido."));
                }

                var newFinancialRequest = new FinancialRequest
                {
                    Request_Status_Id = budgetStatus.Request_State_Id,
                    Reason = "",
                    Value = eventDTO.budget
                };

                // Agregar el nuevo FinancialRequest a la base de datos y guardar cambios para obtener el Request_Id
                _context.FinancialRequests.Add(newFinancialRequest);
                await _context.SaveChangesAsync();

                // Asignar el Request_Id del FinancialRequest creado al evento
                var newEvent = _mapper.Map<Event>(eventDTO);
                newEvent.State_Id = eventState.Event_State_Id;
                newEvent.Financial_Request_Id = newFinancialRequest.Request_Id;
                newEvent.Permission_Id = 1;

                // Agregar el nuevo evento a la base de datos
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                var createdEventDTO = _mapper.Map<EventDTO>(newEvent);

                return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Event_Id }, createdEventDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el evento", ex));
            }
        }


        // PUT: api/Event/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, CreateUpdateEventDTO updatedEvent)
        {
            try
            {
                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID del evento no válido."));
                }

                var state = await _context.EventStates.FirstOrDefaultAsync(s => s.Event_State_Name == updatedEvent.status);
                if (state == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }

                var budgetState = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == updatedEvent.budgetStatus);
                if (state == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }


                var existingFinancialRequest = await _context.FinancialRequests.FirstOrDefaultAsync(s => s.Request_Id == existingEvent.Financial_Request_Id);
                existingFinancialRequest.Request_Status_Id= budgetState.Request_State_Id;
                existingFinancialRequest.Value=updatedEvent.budget;   

                _mapper.Map(updatedEvent, existingEvent);
                existingEvent.State_Id = state.Event_State_Id;

                _context.Entry(existingFinancialRequest).State = EntityState.Modified;
                _context.Entry(existingEvent).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Evento no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el evento",ex));
            }
        }

        // DELETE: api/Event/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var oldEvent = await _context.Events.FindAsync(id);
                if (oldEvent == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Evento no encontrado."));
                }

                _context.Events.Remove(oldEvent);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el evento"));
            }
        }
        /*
        // Método para actualizar el estado del evento
        [HttpPut("{id}/state")]
        public async Task<IActionResult> UpdateEventStatus(int id, [FromBody] string newState)
        {
            try
            {
                var oldEvent = await _context.Events.FindAsync(id);
                if (oldEvent == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Evento no encontrado."));
                }

                var state = await _context.EventStates.FirstOrDefaultAsync(s => s.Event_State_Name == newState);
                if (state == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del estado no válido."));
                }

                oldEvent.State_Id = state.Event_State_Id;

                _context.Entry(oldEvent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el estado del evento"));
            }
        }
        */

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Event_Id == id);
        }
    }


}
