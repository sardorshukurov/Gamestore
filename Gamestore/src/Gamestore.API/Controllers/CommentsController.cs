using Gamestore.BLL.DTOs.Comment;
using Gamestore.BLL.DTOs.Comment.Ban;
using Gamestore.BLL.Services.CommentService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class CommentsController(
    ICommentService commentService) : ControllerBase
{
    [HttpGet("{gameKey}")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAllCommentsByGameKey(string gameKey)
    {
        var comments = await commentService.GetAllCommentsByGameAsync(gameKey);

        return Ok(comments);
    }

    [HttpPost("{gameKey}")]
    public async Task<IActionResult> AddComment(string gameKey, CreateCommentRequest request)
    {
        await commentService.AddCommentAsync(gameKey, request);

        return Ok();
    }

    [HttpGet("ban/durations")]
    public ActionResult<IEnumerable<string>> GetBanDurations()
    {
        var durations = commentService.GetBanDurations();

        return Ok(durations);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        await commentService.DeleteCommentByIdAsync(id);

        return NoContent();
    }

    [HttpPost("ban")]
    public async Task<IActionResult> BanUser(BanUserRequest request)
    {
        await commentService.BanUserAsync(request);
        return NoContent();
    }
}