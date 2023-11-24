using StackExchange.Redis;

namespace Mock.PersonInfo;

public class CacheProvider
{
    public bool CacheEnabled { get; set; }
    private IDatabase _redis { get; set; }
    
    public CacheProvider() {}

    public CacheProvider(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task<string?> GetKey(string key)
    {
        if (!CacheEnabled)
        {
            return null;
        }

        return await _redis.StringGetAsync(key);
    }

    public async Task SetKey(string key, string value)
    {
        if (!CacheEnabled)
        {
            return;
        }

        await _redis.StringSetAsync(key, value);
    }
}