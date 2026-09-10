using ECommerce.Application.Chat.Commands.CreateConversation;
using ECommerce.Application.Chat.Queries.GetConversation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation(
        [FromBody] CreateConversationCommand command)
    {
        var conversationId = await _mediator.Send(command);

        return Ok(new
        {
            conversationId
        });
    }

    [HttpGet("conversations/{conversationId:int}/messages")]
    public async Task<IActionResult> GetMessages(
        int conversationId)
    {
        var messages = await _mediator.Send(
            new GetConversationQuery(conversationId));

        return Ok(messages);
    }
}