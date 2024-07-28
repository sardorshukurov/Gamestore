using Gamestore.BLL.DTOs.Comment;
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
}