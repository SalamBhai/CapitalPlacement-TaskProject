using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskProjectWebAPI.Interfaces.Services;
namespace TaskProjectWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreviewServiceController : ControllerBase
{
    private readonly IPreviewService _previewService;

    public PreviewServiceController(IPreviewService previewService)
    {
        _previewService = previewService;
    }
    [HttpGet("GetPreview")]
    public async Task<IActionResult> GetPreviewAsync([FromQuery] string programTitle)
    {
     var response = await _previewService.GetProgramPreviewAsync(programTitle);
      return response.Status? Ok(response) : BadRequest(response.Message);
    }
}
