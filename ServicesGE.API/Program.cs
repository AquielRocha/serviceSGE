using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServicesGE.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de controllers
builder.Services.AddControllers();

// Configura o DbContext com a string de conexão do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ServicesGE.API",
        Description = "API para o projeto Eletronico",
        Contact = new OpenApiContact
        {
            Name = "Quiel e Brunin",
            Email = "bruno.firmo09@gmail.com",
            Url = new Uri("https://github.com/seurepositorio")
        }
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServicesGE.API V1");
    c.RoutePrefix = ""; // Swagger acessível na raiz
});

// Mapeia os controllers
app.MapControllers();

app.Run();
