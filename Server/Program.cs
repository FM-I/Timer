using Server.Common;
using Server.Hubs;
using Server.Interfaces;
using Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddSingleton<IConnectionsRepository, ConnectionsRepository>();
builder.Services.AddSingleton<TimerManager>();

var app = builder.Build();

app.MapHub<TimerHub>("/timer");

app.Run();
