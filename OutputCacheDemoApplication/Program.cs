using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OutputCacheDemoApplication;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add a reference to the IConnectionMultiplexer to connect to the Redis instance
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost"));

// Add the Output cache to DI container
builder.Services.AddOutputCache();

// Substitute the default implementation of the IOutputCacheStore with the custom
// one using Redis
builder.Services.RemoveAll<IOutputCacheStore>();
builder.Services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

var app = builder.Build();

app.UseOutputCache();

var users = new List<User>()
{
    new User(Guid.NewGuid(), "Boris Becker"),
    new User(Guid.NewGuid(), "FrancescoTotti Biascica")
};

app.MapGet("/users", (string name) =>
{
    return 
        !string.IsNullOrEmpty(name) ? 
            users.Where(u => u.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)) 
            : users;
}).CacheOutput(opt => opt.Tag("users"));

app.MapPost("/users", async (User user, IOutputCacheStore store) =>
{
    users.Add(user);
    await store.EvictByTagAsync("users", CancellationToken.None);
});

app.Run();

internal record User(Guid Id, string Name);