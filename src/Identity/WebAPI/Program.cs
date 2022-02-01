var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApiTypeMapRegistry(registry => registry.RegisterApiTypeMaps());

// Build app
var app = builder.Build();

// Map endpoints
app.MapEndpoints();

// Run app
app.Run();
