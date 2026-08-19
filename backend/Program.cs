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
    if (builder.Environment.IsDevelopment()) {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TestDataService>();
builder.Services.AddScoped<HotelsService>();
builder.Services.AddScoped<HotelsRepository>();
builder.Services.AddScoped<BookingsService>();
builder.Services.AddScoped<BookingsRepository>();

builder.Services.AddCors(options => {
    options.AddPolicy("ReactFrontend", policy => {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.UseCors("ReactFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
