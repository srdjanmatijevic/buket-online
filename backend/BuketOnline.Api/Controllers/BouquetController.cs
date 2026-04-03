using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;
using BuketOnline.Api.DTOs.Bouquets;

namespace BuketOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BouquetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BouquetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BouquetResponseDto>>> Get()
        {
            var bouquets = await _context.Bouquets
                .Include(b => b.Items)
                .ThenInclude(i => i.Flower)
                .ToListAsync();

            var response = bouquets.Select(b => new BouquetResponseDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Items = b.Items.Select(i => new BouquetItemResponseDto
                {
                    Id = i.Id,
                    FlowerId = i.FlowerId,
                    FlowerName = i.Flower != null ? i.Flower.Name : "",
                    FlowerPrice = i.Flower != null ? i.Flower.Price : 0,
                    Quantity = i.Quantity,
                    ItemTotalPrice = (i.Flower != null ? i.Flower.Price : 0) * i.Quantity
                }).ToList(),
                TotalPrice = b.Items.Sum(i => (i.Flower != null ? i.Flower.Price : 0) * i.Quantity)
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BouquetResponseDto>> GetById(int id)
        {
            var bouquet = await _context.Bouquets
                .Include(b => b.Items)
                .ThenInclude(i => i.Flower)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bouquet == null)
                return NotFound();

            var response = new BouquetResponseDto
            {
                Id = bouquet.Id,
                Name = bouquet.Name,
                Description = bouquet.Description,
                Items = bouquet.Items.Select(i => new BouquetItemResponseDto
                {
                    Id = i.Id,
                    FlowerId = i.FlowerId,
                    FlowerName = i.Flower != null ? i.Flower.Name : "",
                    FlowerPrice = i.Flower != null ? i.Flower.Price : 0,
                    Quantity = i.Quantity,
                    ItemTotalPrice = (i.Flower != null ? i.Flower.Price : 0) * i.Quantity
                }).ToList(),
                TotalPrice = bouquet.Items.Sum(i => (i.Flower != null ? i.Flower.Price : 0) * i.Quantity)
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<BouquetResponseDto>> Create(CreateBouquetDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("Buket mora imati bar jednu stavku.");

            var flowerIds = dto.Items.Select(i => i.FlowerId).ToList();

            var flowers = await _context.Flowers
                .Where(f => flowerIds.Contains(f.Id))
                .ToListAsync();

            if (flowers.Count != flowerIds.Count)
                return BadRequest("Jedan ili više cvetova ne postoje.");

            var bouquet = new Bouquet
            {
                Name = dto.Name,
                Description = dto.Description,
                Items = dto.Items.Select(i => new BouquetItem
                {
                    FlowerId = i.FlowerId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _context.Bouquets.Add(bouquet);
            await _context.SaveChangesAsync();

            var createdBouquet = await _context.Bouquets
                .Include(b => b.Items)
                .ThenInclude(i => i.Flower)
                .FirstOrDefaultAsync(b => b.Id == bouquet.Id);

            var response = new BouquetResponseDto
            {
                Id = createdBouquet!.Id,
                Name = createdBouquet.Name,
                Description = createdBouquet.Description,
                Items = createdBouquet.Items.Select(i => new BouquetItemResponseDto
                {
                    Id = i.Id,
                    FlowerId = i.FlowerId,
                    FlowerName = i.Flower != null ? i.Flower.Name : "",
                    FlowerPrice = i.Flower != null ? i.Flower.Price : 0,
                    Quantity = i.Quantity,
                    ItemTotalPrice = (i.Flower != null ? i.Flower.Price : 0) * i.Quantity
                }).ToList(),
                TotalPrice = createdBouquet.Items.Sum(i => (i.Flower != null ? i.Flower.Price : 0) * i.Quantity)
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bouquet = await _context.Bouquets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bouquet == null)
                return NotFound();

            _context.BouquetItems.RemoveRange(bouquet.Items);
            _context.Bouquets.Remove(bouquet);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}