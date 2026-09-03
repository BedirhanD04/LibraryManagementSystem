using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.DataAccess.Data;

// DbContext: EF Core's main class for communicating with the database; each DbSet represents a table
public class AppDbContext : DbContext
{
    // Connection settings (which database, connection string) will be injected here via DI from Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Represents database tables mapped to C# entity models for querying and data operations
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Block author deletion if associated books exist (prevents data loss)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author) // Each book has one author
            .WithMany(a => a.Books) // Each author can have many books
            .HasForeignKey(b => b.AuthorId) // The foreign key in the Books table is AuthorId
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of an author if they have associated books
        

        // If a book is deleted, delete its associated borrowing records
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book) // Each loan is associated with one book
            .WithMany(b => b.Loans) // Each book can have many loans
            .HasForeignKey(l => l.BookId) // The foreign key in the Loans table is BookId
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete: deleting a book deletes its loans


        // If a member is deleted, their associated loan records are also deleted (Cascade Delete)
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Member) // Each loan is associated with one member
            .WithMany(m => m.Loans) // Each member can have many loans
            .HasForeignKey(l => l.MemberId) // The foreign key in the Loans table is MemberId
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete: deleting a member deletes their loans
    }
    
}