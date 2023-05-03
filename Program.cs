using apiChat.Contexto;
using apiChat.Hubs;
using apiChat.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<DatosContexto>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("signalR")
    ?? throw new InvalidOperationException("Connection string 'WebApi' no encontrado."))
) ;
builder.Services.AddSignalR();
var miCor = "Cors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(miCor, policy => {
        policy.AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseCors(miCor);

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ConexionesHub>("/conexionesHub");
});

app.Run();


