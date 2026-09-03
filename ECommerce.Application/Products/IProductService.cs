using ECommerce.Application.Products.DTOs;

namespace ECommerce.Application.Products;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductResponse> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task UpdateProductAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(int id, CancellationToken cancellationToken = default);
}
