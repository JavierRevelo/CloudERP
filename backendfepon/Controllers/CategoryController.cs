using backendfepon.Data;
using backendfepon.DTOs.CareerDTOs;
using backendfepon.DTOs.CategoryDTOs;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryDTO
                    {
                        description = c.Description
                    })
                    .ToListAsync();

                return Ok(categories);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la categoría"));
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Where(p => p.Category_Id == id)
                    .Select(p => new CategoryDTO
                    {
                        description = p.Description
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Categoría no encontrada."));
                }

                return Ok(category);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la categoría"));
            }
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreateCategoryDTO categoryDTO)
        {
            try
            {
                var category = new Category
                {
                    Description = categoryDTO.Description
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = category.Category_Id }, category);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar la categoría"));
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Categoría no encontrada."));
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar la categoría"));
            }
        }
    }
}
