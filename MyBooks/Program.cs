using Microsoft.EntityFrameworkCore;
using MyBooks.Data;
using MyBooks.Data.Services;
using MyBooks.Exceptions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});
string ConnectionString = builder.Configuration.GetConnectionString("DafaultConnectionString");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();

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

app.UseAuthorization();
//Error Handling
app.ConfigureBuildInExceptionHandler(loggerFactory);
//app.ConfigureCustomExceptionHandler();
app.MapControllers();
//AddDbInitializer.Seed(app);
app.Run();
