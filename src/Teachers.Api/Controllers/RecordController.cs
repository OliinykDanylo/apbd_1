using Microsoft.AspNetCore.Mvc;
using Teachers.Application;

namespace Teachers.Api.Controllers;

[ApiController]
[Route("api")]
public class RecordController : ControllerBase
{
    private readonly IRecordService _recordService;
    
    public RecordController(IRecordService recordService)
    {
        _recordService = recordService;
    }
    
    [HttpGet("records")]
    public IResult GetAll()
    {
        try
        {
            var adventurers = _recordService.GetAllRecordsAsync();
            return Results.Ok(adventurers);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: 500);
        }
    }
    
    [HttpPost("records")]
    public IResult CreateRecord([FromBody] Record record)
    {
        try
        {
            var (success, error) = _recordService.CreateRecordAsync(record).GetAwaiter().GetResult();
            if (!success)
                return Results.BadRequest(error);

            return Results.Created($"/api/records/{record.Id}", record);
        }
        catch (UnauthorizedAccessException)
        {
            return Results.StatusCode(403);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: 500);
        }
    }
}