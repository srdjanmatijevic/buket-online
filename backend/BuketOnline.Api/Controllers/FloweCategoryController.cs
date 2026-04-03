using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;

namespace BuketOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowerCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlowerCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlowerCategory>>> Get()
        {
            var categories = await _context.FlowerCategories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlowerCategory>> GetById(int id)
        {
            var category = await _context.FlowerCategories.FindAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<FlowerCategory>> Create(FlowerCategory category)
        {
            _context.FlowerCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.FlowerCategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FlowerCategory updatedCategory)
        {
            if (id != updatedCategory.FlowerCategoryId)
                return BadRequest();

            var category = await _context.FlowerCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.FlowerCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.FlowerCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}