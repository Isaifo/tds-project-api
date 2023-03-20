

using tds_week1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var model = new List<Model>
{
    new Model { Id = 1, Title = "Task 1" , Description = "Description 1", Done = true },
    new Model { Id = 2, Title = "Task 2" , Description = "Description 2", Done = true },
    new Model { Id = 3, Title = "Task 3" , Description = "Description 3", Done = true }

};

builder.Services.AddSingleton(model);
builder.Services.AddSwaggerGen( c=> {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Model", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// adding routes

app.MapGet("/models", () =>

{
    var modelService = app.Services.GetRequiredService<List<Model>>();
    return Results.Ok(modelService);
});

app.MapGet("/models/{id}", (int id, HttpRequest request) =>

{
    var modelService = app.Services.GetRequiredService<List<Model>>();
    var model = modelService.FirstOrDefault(m => m.Id == id);

    if (model == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(model);

});


app.MapPost("/models/create", (Task model) =>

{ 
  //  var modelService = app.Services.GetRequiredService<List<Model>>();
    // model.Id = modelService.Max(t => t.Id) + 1;
   // modelService.Add(model);
});


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};






app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
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

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
