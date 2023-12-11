namespace DTOs.Products
{
    public class ProductsAndCategoriesDTO
    {
        public string CategoryName { get; set; }
        public List<ProductsDTO> Items { get; set; }
    }
}
