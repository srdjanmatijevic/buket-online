namespace BuketOnline.Api.Models
{
    public class Bouquet
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public List<BouquetItem> Items { get; set; } = new();
    }
}