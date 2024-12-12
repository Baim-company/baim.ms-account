using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Agiles.Comments;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Interfaces;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    //[Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> AddCommentAsync([FromBody] CreateCommentModel model)
    {
        if (model == null) return BadRequest("Invalid comment data.");

        var response = await _commentService.AddCommentAsync(model);
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response);
    }


    //[Authorize]
    [HttpGet("Ticket/{ticketId}")]
    public async Task<IActionResult> GetCommentsByTicketId(Guid ticketId)
    {
        var response = await _commentService.GetCommentsByTicketIdAsync(ticketId);
        if (response.Data == null) return NotFound(new Response<string>("No comments found for the given ticket ID", null));

        return Ok(new Response<object>("Comments retrieved successfully", response.Data));
    }


    //[Authorize]
    [HttpGet("SubTicket/{subTicketId}")]
    public async Task<IActionResult> GetCommentsBySubTicketId(Guid subTicketId)
    {
        var response = await _commentService.GetCommentsBySubTicketIdAsync(subTicketId);
        
        if (response.Data == null) return NotFound(new Response<string>("No comments found for the given sub-ticket ID", null));

        return Ok(new Response<object>("Comments retrieved successfully", response.Data));
    }


    //[Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteComment([FromHeader]Guid commentId)
    {
        var response = await _commentService.DeleteCommentByIdAsync(commentId);

        if (response.Data == null) return NotFound(response.Message);

        return Ok(response.Message);
    }


    //[Authorize]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentModel model)
    {
        if (model == null) return BadRequest("Invalid comment update data.");

        var response = await _commentService.UpdateCommentAsync(model);
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response);
    }


    //[Authorize]
    [HttpPost("Add")]
    public async Task<IActionResult> AddReactionAsync([FromHeader]Guid commentId, [FromBody] AddReactionModel model)
    {
        if (model == null) return BadRequest("Invalid reaction data.");

        var response = await _commentService.AddReactionAsync(commentId, model);
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response);
    }


    //[Authorize]
    [HttpDelete("Delete/Comment/{commentId}/reaction/{reactionId}")]
    public async Task<IActionResult> RemoveReactionAsync(Guid commentId, Guid reactionId)
    {
        var response = await _commentService.RemoveReactionAsync(commentId, reactionId);
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response);
    }


    //[Authorize]
    [HttpPut("Update/Reaction/{reactionId}")]
    public async Task<IActionResult> UpdateReactionAsync(Guid reactionId, [FromBody] string newEmoji)
    {
        if (string.IsNullOrEmpty(newEmoji)) return BadRequest("Invalid emoji data.");

        var response = await _commentService.UpdateReactionAsync(reactionId, newEmoji);
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response);
    }
}