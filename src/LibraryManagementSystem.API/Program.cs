using LibraryManagementSystem.DataAccess.Data;
using LibraryManagementSystem.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Business.Services;
using LibraryManagementSystem.API;

var builder = WebApplication.CreateBuilder(args);

// Registering AppDbContext in the DI container — specifying that it will use SQLite and where to get the connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the generic repository — resolve IRepository<Author> as Repository<Author> whenever it's requested
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register the Loan-specific repository
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// Register the business services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Supports standard ASP.NET Core error JSON format

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(); // Must be placed first in the pipeline to catch everything downstream

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();