using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;

namespace BuketOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlowerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flower>>> Get()
        {
            var flowers = await _context.Flowers
                .Include(f => f.FlowerCategory)
                .ToListAsync();

            return Ok(flowers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flower>> GetById(int id)
        {
            var flower = await _context.Flowers
                .Include(f => f.FlowerCategory)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flower == null)
                return NotFound();

            return Ok(flower);
        }

        [HttpPost]
        public async Task<ActionResult<Flower>> Create(Flower flower)
        {
            var categoryExists = await _context.FlowerCategories
                .AnyAsync(c => c.FlowerCategoryId == flower.FlowerCategoryId);

            if (!categoryExists)
                return BadRequest("Kategorija ne postoji.");

            _context.Flowers.Add(flower);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = flower.Id }, flower);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Flower updatedFlower)
        {
            if (id != updatedFlower.Id)
                return BadRequest();

            var flower = await _context.Flowers.FindAsync(id);
            if (flower == null)
                return NotFound();

            var categoryExists = await _context.FlowerCategories
                .AnyAsync(c => c.FlowerCategoryId == updatedFlower.FlowerCategoryId);

            if (!categoryExists)
                return BadRequest("Kategorija ne postoji.");

            flower.Name = updatedFlower.Name;
            flower.Price = updatedFlower.Price;
            flower.ImageUrl = updatedFlower.ImageUrl;
            flower.FlowerCategoryId = updatedFlower.FlowerCategoryId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flower = await _context.Flowers.FindAsync(id);
            if (flower == null)
                return NotFound();

            _context.Flowers.Remove(flower);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}