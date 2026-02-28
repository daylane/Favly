using Favly.api.Extensions;
using Favly.api.Middleware;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ResolveDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Host.UseWolverine(opts =>
//{
//    // Configura o Outbox com EF Core para garantir consistência total
//    opts.UseEntityFrameworkCoreOutbox<dbontext>();
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "favly v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
