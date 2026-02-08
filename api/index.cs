using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// This follows the Minimal APIs pattern introduced in .NET 6+
var builder = WebApplication.CreateBuilder(args);

// 1. Add your existing services (Models, DB Contexts, etc.)
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 2. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 3. Map your existing Controllers
app.MapControllers();

// 4. Fallback route to test if the API is alive
app.MapGet("/", () => "MyResumeApi is running on Vercel!");

app.Run();