namespace BuketOnline.Api.DTOs.Bouquets
{
    public class CreateBouquetDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public List<CreateBouquetItemDto> Items { get; set; } = new();
    }
}