using LibraryManagementSystem.DataAccess.Data;
using LibraryManagementSystem.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Business.Services;

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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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