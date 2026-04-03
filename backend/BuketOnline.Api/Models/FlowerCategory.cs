namespace BuketOnline.Api.Models
{
    public class FlowerCategory
    {
        public int FlowerCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Flower> Flowers { get; set; } = new();
    }
}