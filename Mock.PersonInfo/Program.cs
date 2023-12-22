using Mock.PersonInfo;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    var redisConnectionOptions = ConfigurationOptions.Parse("redis");
    redisConnectionOptions.Password = Environment.GetEnvironmentVariable("REDIS_SECRET");
    redisConnectionOptions.ConnectTimeout = 1000;

    builder.Services.AddSingleton<CacheProvider>(new CacheProvider(ConnectionMultiplexer.Connect(redisConnectionOptions))
    {
        CacheEnabled = true
    });
}
catch
{
    builder.Services.AddSingleton<CacheProvider>(new CacheProvider() { CacheEnabled = false });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();