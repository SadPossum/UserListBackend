using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using UserListBackend;
using UserListBackend.Data;
using UserListBackend.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add builder.Services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.
                       GetConnectionString("DefaultConnection")));

builder.Services.AddCors();

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(p => p.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("Content-Disposition"));

app.UseMiddleware<CustomExceptionMiddleware>();

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
