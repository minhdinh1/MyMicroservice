using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using MyMicroservice.DbContextSpace;
using MyMicroservice.Jobs;
using MyMicroservice.Jobs.PersistRatesJob;
using MyMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddHostedService<JobHostedService>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("WebApiDatabase"));
    }));

builder.Services.AddHangfireServer();
builder.Services.AddPersistRatesJobServices();
builder.Services.AddTransient<IRatesService, RatesService>();
var app = builder.Build();
using var db = new BloggingContext(builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


app.UseHangfireDashboard("/hangfire");

// Create
app.MapPost("/blog", () => {
    Console.WriteLine("Inserting a new blog");
    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
    db.SaveChanges();
    return Results.Accepted();
});

// Read
app.MapGet("/blogs", () => {
    Console.WriteLine("Querying for blogs");
    var blogs = db.Blogs
        .Include(b => b.Posts)
        .ToList();
    return Results.Ok(blogs);
});

app.MapGet("/blog/{id}", (int id) => {
    Console.WriteLine("Querying for a blog");
    var blog = db.Blogs
        .Where(b => b.BlogId == id)
        .Include(b => b.Posts)
        .FirstOrDefault();
    if (blog is null) return Results.NotFound($"Couldn't find blog with id: {id}");
    return Results.Ok(blog);
});

// Update
app.MapPatch("/blog/{id}", (int id) => {
    Console.WriteLine("Updating the blog and adding a post");
    var blog = db.Blogs
        .Where(b => b.BlogId == id)
        .FirstOrDefault();
    if (blog is null) return Results.NotFound($"Couldn't find blog with id: {id}");
    blog.Url = "https://devblogs.microsoft.com/dotnet";
    blog.Posts.Add(
        new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
    db.SaveChanges();
    return Results.Accepted();
});


// Delete
app.MapDelete("/blog/{id}", (int id) => {
    Console.WriteLine("Delete the blog");
    var blog = db.Blogs
        .Where(b => b.BlogId == id)
        .FirstOrDefault();
    if (blog is null) return Results.NotFound($"Couldn't find blog with id: {id}");
    db.Remove(blog);
    db.SaveChanges();
    return Results.Accepted();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/rate/{symbol}", (string symbol) => {
    Console.WriteLine($"Querying for last {symbol} rate");
    var rate = db.Rates
        .Where(b => b.Symbol == symbol.ToUpper())
        .OrderByDescending(b => b.CreatedDateTime)
        .FirstOrDefault();
    if (rate is null) return Results.NotFound($"Couldn't find {symbol} rate");
    return Results.Ok(rate);
});

app.MapGet("/rates/{symbol}", (string symbol) => {
    Console.WriteLine($"Querying for last {symbol} rate");
    var rate = db.Rates
        .Where(b => b.Symbol == symbol.ToUpper())
        .ToList();
    if (rate is null) return Results.NotFound($"Couldn't find {symbol} rate");
    return Results.Ok(rate);
});

app.MapGet("/rates", () => {
    Console.WriteLine("Querying for rates");
    var rate = db.Rates.ToList();
    if (rate is null) return Results.NotFound($"Couldn't find rates");
    return Results.Ok(rate);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}