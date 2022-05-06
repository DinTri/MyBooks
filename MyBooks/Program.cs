using Microsoft.EntityFrameworkCore;
using MyBooks.Data;
using MyBooks.Data.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
try
{
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error during initialization");
}
finally
{
    Log.Information("App finished");
    Log.CloseAndFlush();
}

string ConnectionString = builder.Configuration.GetConnectionString("DafaultConnectionString");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();
builder.Services.AddTransient<LogsService>();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");
    //config.ApiVersionReader = new MediaTypeApiVersionReader();
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
app.UseSerilogRequestLogging();
app.UseAuthorization();
//Error Handling
//app.ConfigureBuildInExceptionHandler(loggerFactory);
app.MapControllers();
app.Run();
