using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Mock.PersonInfo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PersonInfoController : ControllerBase
{
    private readonly ILogger<PersonInfoController> _logger;
    private readonly IDatabase _redis;

    public PersonInfoController(
        ILogger<PersonInfoController> logger,
        IConnectionMultiplexer redisMultiplexer)
    {
        _logger = logger;
        _redis = redisMultiplexer.GetDatabase();
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
        
        _logger.LogDebug(JsonSerializer.Serialize(personInfo));

        (await _redis.StringGetAsync("numberOfCalls")).TryParse(out int numberOfCalls);
        await _redis.StringSetAsync("numberOfCalls", numberOfCalls + 1);
        
        return personInfo;
    }
}