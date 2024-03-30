using GameStore.API.Router;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGameRouter();

app.Run();
