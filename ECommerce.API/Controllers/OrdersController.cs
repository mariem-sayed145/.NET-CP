using ECommerce.Application.Orders.Commands.CancelOrder;
using ECommerce.Application.Orders.Commands.CheckoutOrder;
using ECommerce.Application.Orders.DTOs;
using ECommerce.Application.Orders.Queries.GetCustomerOrders;
using ECommerce.Application.Orders.Queries.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class OrdersController : BaseApiController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);

        var order = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(order);
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetCustomerOrders(
        int customerId,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerOrdersQuery(customerId);

        var orders = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(orders);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutOrderResponse>> Checkout(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CheckoutOrderCommand(
            request.CustomerId,
            request.Items,
            request.CouponCode);

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("cancel/{id:int}")]
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(id);

        await _mediator.Send(
            command,
            cancellationToken);

        return Ok(new
        {
            message = "Order cancelled successfully."
        });
    }
}