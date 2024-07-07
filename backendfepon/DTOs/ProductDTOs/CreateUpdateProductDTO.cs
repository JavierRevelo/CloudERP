﻿namespace backendfepon.DTOs.ProductDTOs
{
    public class CreateUpdateProductDTO
    {

        public string Category_Name { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal? Economic_Value { get; set; }

        public int Quantity { get; set; }

        public string Label { get; set; } = null!;

        public string Provider_Name { get; set; }
    }
}
