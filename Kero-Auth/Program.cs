using Kero_Auth.Application.Services.ServiceCollection;
using Kero_Auth.Configuration.Extensions;
using Kero_Auth.Infrastructure.Services.ServiceCollection;
using Kero_Auth.Middlewares;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.ConfigureSwagger();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    app.AddSwaggerUi();
}

app.UseHttpsRedirection();

app.UseMiddleware<FirebaseAuthExceptionHandlerMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
