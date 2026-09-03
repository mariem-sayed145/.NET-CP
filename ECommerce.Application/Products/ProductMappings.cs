using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Products;

public static class ProductMappings
{
    public static ProductResponse ToResponse(this Product product) =>
        new(product.Id, product.Name, product.SKU, product.Price, product.StockQuantity);

    public static IReadOnlyList<ProductResponse> ToResponseList(this IEnumerable<Product> products) =>
        products.Select(ToResponse).ToList().AsReadOnly();
}
