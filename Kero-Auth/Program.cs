using Kero_Auth.Configuration.Extensions;
using Kero_Auth.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.AddSwaggerUi();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("X-Pagination-Total-Pages")
           .WithExposedHeaders("X-Pagination-Next-Page")
           .WithExposedHeaders("X-Pagination-Has-Next-Page")
           .WithExposedHeaders("X-Pagination-Total-Pages");
});

app.UseMiddleware<FirebaseAuthExceptionHandlerMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
