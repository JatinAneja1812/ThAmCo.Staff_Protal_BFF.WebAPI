using DTOs.Products;

namespace Service.Interfaces.Products
{
    public interface IProductsService
    {
        public List<ProductsDTO> GetAllAvailableProducts();
        public List<ProductsDTO> GetAllCategoryProducts(string categoryName);
    }
}
