using backendfepon.Data;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.DTOs.ProviderDTOs;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ProviderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDTO>>> GetProviders()
        {
            try
            {
                var providers = await _context.Providers
                    .Select(p => new ProviderDTO
                    {
                        id = p.Provider_Id,
                        name = p.Name,
                        phone = p.Phone,
                        email = p.Email
                    })
                    .ToListAsync();

                return Ok(providers);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obetener el proveedor."));
            }
        }

        // GET: api/Provider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderDTO>> GetProvider(int id)
        {
            try
            {
                var provider = await _context.Providers
                    .Where(p => p.Provider_Id == id)
                    .Select(p => new ProviderDTO
                    {
                        id = p.Provider_Id,
                        name = p.Name,
                        phone = p.Phone,
                        email = p.Email
                    })
                    .FirstOrDefaultAsync();

                if (provider == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Proveedor no encontrado."));
                }

                return Ok(provider);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el proveedor."));
            }
        }

        // POST: api/Provider
        [HttpPost]
        public async Task<ActionResult<ProviderDTO>> PostProvider(CreateUpdateProviderDTO providerDTO)
        {
            try
            {
                var provider = new Provider
                {
                    Name = providerDTO.Name,
                    Phone = providerDTO.Phone,
                    Email = providerDTO.Email
                };

                _context.Providers.Add(provider);
                await _context.SaveChangesAsync();

                var createdProviderDTO = new ProviderDTO
                {
                    id = provider.Provider_Id,
                    name = provider.Name,
                    phone = provider.Phone,
                    email = provider.Email
                };

                return CreatedAtAction(nameof(GetProvider), new { id = provider.Provider_Id }, createdProviderDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el servidor"));
            }
        }

        // PUT: api/Providers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvider(int id, CreateUpdateProviderDTO updatedProvider)
        {
            try
            {
                var provider = await _context.Providers.FindAsync(id);
                if (provider == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID de proveedor no válido."));
                }

                provider.Name = updatedProvider.Name;
                provider.Phone = updatedProvider.Phone;
                provider.Email = updatedProvider.Email;

                _context.Entry(provider).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Proveedor no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el proveedor"));
            }
        }

        // DELETE: api/Provider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            try
            {
                var provider = await _context.Providers.FindAsync(id);
                if (provider == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Proveedor no encontrado."));
                }

                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el proveedor."));
            }
        }

        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.Provider_Id == id);
        }
    }
}
