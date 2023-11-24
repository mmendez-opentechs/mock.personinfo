using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Mock.PersonInfo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PersonInfoController : ControllerBase
{
    private readonly ILogger<PersonInfoController> _logger;
    private readonly CacheProvider _cacheProvider;

    public PersonInfoController(
        ILogger<PersonInfoController> logger,
        CacheProvider cacheProvider)
    {
        _logger = logger;
        _cacheProvider = cacheProvider;
    }

    [HttpGet(Name = "GetPersonInfo")]
    public async Task<PersonInfo> Get()
    {
        var personInfo =  new PersonInfo()
        {
            BirthDate = DateTime.Now.AddYears(-10),
            FirstName = "Meagan",
            PersonNumber = "54523"
        };
        
        _logger.LogInformation(JsonSerializer.Serialize(personInfo));

        if (_cacheProvider.CacheEnabled)
        {
            int.TryParse((await _cacheProvider.GetKey("numberOfCalls")), out int numberOfCalls);
            await _cacheProvider.SetKey("numberOfCalls", (numberOfCalls + 1).ToString());
            int.TryParse((await _cacheProvider.GetKey("numberOfCalls")), out int numberOfCallsUpdated);
            personInfo.NumberOfTimesAccessed = numberOfCallsUpdated;
        }
        
        return personInfo;
    }
}