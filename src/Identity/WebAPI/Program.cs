using Cofi.Identity.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapEndpoints()
    .User();

app.Run();
