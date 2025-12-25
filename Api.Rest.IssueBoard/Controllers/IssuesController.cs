using Api.Rest.IssueBoard.Data;
using Api.Rest.IssueBoard.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Rest.IssueBoard;

namespace Api.Rest.IssueBoard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly IssuesDbContext _context;
    private readonly ILogger<IssuesController> _logger;

    public IssuesController(IssuesDbContext context, ILogger<IssuesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IssueDto>>> GetIssues()
    {
        try
        {
            var issues = await _context.Issues
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return Ok(issues.Select(i => i.ToDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting issues");
            return StatusCode(500, "An error occurred while retrieving issues");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IssueDto>> GetIssue(int id)
    {
        try
        {
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting issue {IssueId}", id);
            return StatusCode(500, "An error occurred while retrieving the issue");
        }
    }

    [HttpPost]
    public async Task<ActionResult<IssueDto>> CreateIssue(CreateIssueDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issue = dto.ToModel();
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIssue), new { id = issue.Id }, issue.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating issue");
            return StatusCode(500, "An error occurred while creating the issue");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, UpdateIssueDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            issue.UpdateFromDto(dto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating issue {IssueId}", id);
            return StatusCode(500, "An error occurred while updating the issue");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(int id)
    {
        try
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting issue {IssueId}", id);
            return StatusCode(500, "An error occurred while deleting the issue");
        }
    }
}
