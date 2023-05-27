using JWTAuthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            List<Category> items = await _context.Categories.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var item = await _context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound("Category with #" + id + " not found.");
            }
            return Ok(item);
        }

        [HttpPost(Name = "CreateCategory")]
        public async Task<ActionResult<Category>> Create(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpPut(Name = "UpdateCategory")]
        public async Task<ActionResult<Category>> Update(Category request)
        {
            var entity = await _context.Categories.FindAsync(request.Id);
            if (entity == null)
            {
                return BadRequest("Category with #" + request.Id + " not found.");
            }
            entity.label = request.label;
            entity.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var item = await _context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound("Category with #" + id + " not found.");
            }
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Category " + id + " removed successfully.");
        }
    }
}
