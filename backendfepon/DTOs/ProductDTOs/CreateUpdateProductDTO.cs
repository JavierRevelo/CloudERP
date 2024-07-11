namespace backendfepon.DTOs.ProductDTOs
{
    public class CreateUpdateProductDTO
    {

        public string category { get; set; }

        public string name { get; set; } = null!;

        public string description { get; set; } = null!;

        public decimal? price { get; set; }

        public int quantity { get; set; }

        public string label { get; set; } = null!;

        public string provider { get; set; }
    }
}
