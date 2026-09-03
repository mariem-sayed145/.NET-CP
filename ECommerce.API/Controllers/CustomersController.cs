using ECommerce.Application.Customers.Commands.RegisterCustomer;
using ECommerce.Application.Customers.Commands.UpgradeToVip;
using ECommerce.Application.Customers.DTOs;
using ECommerce.Application.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class CustomersController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(id);

        var customer = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(
            request.FullName,
            request.Email,
            request.IsVip);

        var created = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPost("{id:int}/upgrade-vip")]
    public async Task<IActionResult> UpgradeVip(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new UpgradeToVipCommand(id);

        await _mediator.Send(
            command,
            cancellationToken);

        return Ok(new
        {
            message = "Customer upgraded to VIP successfully."
        });
    }
}