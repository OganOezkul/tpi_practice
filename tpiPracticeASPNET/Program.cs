using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using tpiPracticeASPNET;
using tpiPracticeASPNET.Hubs;
using tpiPracticeClasses;

const string SIGNAL_R_NOTIFICATION_HUB_PATH = "/notifications";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TpiDB>(opt => opt.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TpiDB; Trusted_Connection = True; "));

builder.Services.AddSignalR();

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(o => {
    o.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>(SIGNAL_R_NOTIFICATION_HUB_PATH);


app.Run();