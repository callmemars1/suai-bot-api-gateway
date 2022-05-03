using Microsoft.AspNetCore.Mvc;

namespace suai_api.Controllers;

[ApiController()]
[Route("[controller].[action]")]
public class TeacherInfoController : ControllerBase 
{
    private readonly ILogger<TeacherInfoController> _logger;

    public TeacherInfoController(ILogger<TeacherInfoController> logger)
    {
        _logger = logger;
    }
}