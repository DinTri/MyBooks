using Microsoft.EntityFrameworkCore;
using MyBooks.Data;
using MyBooks.Data.Services;

var builder = WebApplication.CreateBuilder(args);
string ConnectionString = builder.Configuration.GetConnectionString("DafaultConnectionString"); 
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddTransient<BookService>();
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

app.MapControllers();
//AddDbInitializer.Seed(app);
app.Run();
