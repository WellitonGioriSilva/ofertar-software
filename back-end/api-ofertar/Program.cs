using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add necessary services
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ofertar API",
        Version = "v1",
        Description = "API for the Ofertar System, responsible for managing thithes",
        Contact = new OpenApiContact
        {
            Name = "Welliton Giori Silva",
            Email = "gioriwelliton47@gmail.com",
            Url = new Uri("https://github.com/WellitonGioriSilva")
        }
    });
});

var app = builder.Build();

// Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ofertar API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz (ex: https://localhost:5001/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
