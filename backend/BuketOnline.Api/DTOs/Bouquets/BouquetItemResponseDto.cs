namespace BuketOnline.Api.DTOs.Bouquets
{
    public class BouquetItemResponseDto
    {
        public int Id { get; set; }
        public int FlowerId { get; set; }
        public string FlowerName { get; set; } = "";
        public decimal FlowerPrice { get; set; }
        public int Quantity { get; set; }
    }
}