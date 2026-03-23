using Microsoft.EntityFrameworkCore;
using MyNotes.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок. Додаємо перевірку, щоб побачити помилку відразу
var connectionString = builder.Configuration.GetConnectionString("Database");

if (string.IsNullOrEmpty(connectionString))
{
    // Якщо ви побачите цей текст у консолі, значить програма не бачить ваш JSON
    throw new Exception("ПОМИЛКА: Рядок підключення 'Database' не знайдено в appsettings.json!");
}

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Створення бази
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();