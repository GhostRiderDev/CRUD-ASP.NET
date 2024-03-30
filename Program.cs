using GameStore.API.Data;
using GameStore.API.Router;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlServer<GameStoreContext>(connString);

var app = builder.Build();

app.MapGameRouter();
app.MigrateDb();

app.Run();
