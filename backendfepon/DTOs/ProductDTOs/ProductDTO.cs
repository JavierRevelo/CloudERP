namespace backendfepon.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int id { get; set; }
        public int stateid { get; set; }

        public string category { get; set; }

        public string name { get; set; } = null!;

        public string description { get; set; } = null!;

        public decimal? price { get; set; }

        public int quantity { get; set; }

        public string label { get; set; } = null!;


        public string provider { get; set; }
    }
}
