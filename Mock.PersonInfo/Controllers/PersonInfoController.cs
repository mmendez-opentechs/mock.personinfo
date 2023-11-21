using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Mock.PersonInfo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PersonInfoController : ControllerBase
{
    private readonly ILogger<PersonInfoController> _logger;

    public PersonInfoController(ILogger<PersonInfoController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetPersonInfo")]
    public PersonInfo Get()
    {
        var personInfo =  new PersonInfo()
        {
            BirthDate = DateTime.Now.AddYears(-10),
            FirstName = "Meagan",
            PersonNumber = "54523"
        };
        
        _logger.LogDebug(JsonSerializer.Serialize(personInfo));
        return personInfo;
    }
}