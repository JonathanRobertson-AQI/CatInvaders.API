using System.Reflection;
using CatInvaders.API.Data;
using CatInvaders.API.Data.Repositories;
using CatInvaders.API.Middleware;
using CatInvaders.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── OpenAPI / Swagger ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Cat Invaders API",
        Version = "v1",
        Description = "RESTful API for storing and retrieving Cat Invaders high scores."
    });

    // Include XML documentation in Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<CatInvadersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Application Services ──────────────────────────────────────────────────────
builder.Services.AddScoped<IHighScoreRepository, HighScoreRepository>();
builder.Services.AddScoped<IHighScoreService, HighScoreService>();

// ── Build ─────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Apply EF Migrations on startup ────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatInvadersDbContext>();
    db.Database.Migrate();
}

// ── Middleware Pipeline ───────────────────────────────────────────────────────
// Global exception handler must be first so it wraps every subsequent middleware.
app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cat Invaders API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expose Program for integration testing (WebApplicationFactory)
public partial class Program { }
