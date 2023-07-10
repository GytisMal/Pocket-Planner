global using Microsoft.EntityFrameworkCore;
global using PocketPlanner.Data;
global using PocketPlanner.Services.CategoriesService;
global using PocketPlanner.Dtos.Categories;
global using PocketPlanner.Services.BudgetService;
global using PocketPlanner.Dtos.Budget;
global using PocketPlanner.Services.TransactionService;
global using PocketPlanner.Dtos.Transactions;
using Microsoft.Extensions.DependencyInjection;
using PocketPlanner;
using PocketPlanner.Data;
using PocketPlanner.Services.BudgetService;
using PocketPlanner.Services.CategoriesService;
using PocketPlanner.Services.TransactionService;
using PocketPlanner;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition")
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
