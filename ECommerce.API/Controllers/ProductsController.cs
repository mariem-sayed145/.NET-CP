using ECommerce.Application.Products.Commands.CreateProduct;
using ECommerce.Application.Products.Commands.DeleteProduct;
using ECommerce.Application.Products.Commands.UpdateProduct;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Queries.GetAllProducts;
using ECommerce.Application.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class ProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetAllProductsQuery();

        var products = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var product = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.SKU,
            request.Price,
            request.StockQuantity);

        var created = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.SKU,
            request.Price,
            request.StockQuantity);

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }
}