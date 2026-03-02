using MediatR;
using Microsoft.AspNetCore.Mvc;
using TextAnalyzer.API.Models;
using TextAnalyzer.Application.DTOs;
using TextAnalyzer.Application.Queries.AnalyzeText;

namespace TextAnalyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalysisController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(AnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Analyze(
        [FromBody] AnalyzeTextRequest request,
        CancellationToken cancellationToken)
    {
        var query = new AnalyzeTextQuery(request.Text, request.Language);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}