using APBD_s26126_Hospital_dbfirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s26126_Hospital_dbfirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? search)
    {
        var res = await _dbService.GetPatientsAsync(search);
        return Ok(res);
    }
}