


using Microsoft.EntityFrameworkCore;
using API_YTB.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<ProductContext>(opt => 
    opt.UseMySql(
        builder.Configuration.GetConnectionString("TodoContext"),

        new MySqlServerVersion(new Version(8, 0, 21)))
        );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
