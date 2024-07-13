using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backendfepon.Utils;


namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.State)
                    .Include(p => p.Provider)
                    .Include(p => p.Category)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new ProductDTO
                    {
                        id = p.Product_Id,
                        stateid = p.State_Id,
                        name = p.Name,

                        description = p.Description,
                        price = p.Economic_Value,
                        quantity = p.Quantity,
                        label = p.Label,
                        category = p.Category.Description,
                        provider = p.Provider.Name
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los productos"));
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.State)
                    .Include(p => p.Provider)
                    .Include(p => p.Category)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE && p.Product_Id == id)
                    .Select(p => new ProductDTO
                    {
                        id = p.Product_Id,
                        stateid = p.State_Id,

                        name = p.Name,
                        description = p.Description,
                        price = p.Economic_Value,
                        quantity = p.Quantity,
                        label = p.Label,
                        category = p.Category.Description,
                        provider = p.Provider.Name
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Producto no encontrado."));
                }

                return Ok(product);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los productos"));
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(CreateUpdateProductDTO productDTO)
        {
            try
            {
                var productExist = await _context.Products.FirstOrDefaultAsync(c => c.Name == productDTO.name);
                if (productExist.Name == productDTO.name)
                {
                    return BadRequest(GenerateErrorResponse(400, "El producto ya existe."));
                }

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Description == productDTO.category);
                if (category == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de categoría no válido."));
                }

                var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Name == productDTO.provider);
                if (provider == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de proveedor no válido."));
                }

                var product = _mapper.Map<Product>(productDTO);
                product.State_Id = Constants.DEFAULT_STATE;
                product.Category_Id = category.Category_Id;
                product.Provider_Id = provider.Provider_Id;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var createdProductDTO = _mapper.Map<ProductDTO>(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Product_Id }, createdProductDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el producto"));
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CreateUpdateProductDTO updatedProductDTO)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Producto no encontrado."));
                }


                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Description == updatedProductDTO.category);
                if (category == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de categoría no válido."));
                }

                var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Name == updatedProductDTO.provider);
                if (provider == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de proveedor no válido."));
                }

                _mapper.Map(updatedProductDTO, product);
                product.Category_Id = category.Category_Id;
                product.Provider_Id = provider.Provider_Id;

                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Producto no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el producto."));
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Producto no encontrado."));
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el producto."));
            }
        }

        // PATCH: api/Products/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProductState(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Producto no encontrado."));
                }

                product.State_Id = Constants.STATE_INACTIVE;
                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Producto no encontrado."));
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Product_Id == id);
        }
    }
}
