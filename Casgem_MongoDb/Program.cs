using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.Configure<EstateStoreDatabaseSettings>(
    builder.Configuration.GetSection(nameof(EstateStoreDatabaseSettings)));

builder.Services.AddSingleton<IEstateStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<EstateStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("EstateStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IEstateService, EstateService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
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

//app.UseCors(builder =>
//{
//    builder.WithOrigins("https://localhost:7217")
//           .AllowAnyHeader()
//           .AllowAnyMethod();
//});

app.UseAuthorization();

app.MapControllers();

app.Run();
