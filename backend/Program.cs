using Microsoft.EntityFrameworkCore;
using BookAStay.Data;
using BookAStay.Services;
using BookAStay.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TestDataService>();
builder.Services.AddScoped<HotelsService>();
builder.Services.AddScoped<HotelsRepository>();
builder.Services.AddScoped<BookingsService>();
builder.Services.AddScoped<BookingsRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

//if (app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
