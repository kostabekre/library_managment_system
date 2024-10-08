using LibraryManagementSystemAPI.Authors.Models;
using LibraryManagementSystemAPI.Books.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Context;

public class BookContext : IdentityDbContext
{
    public BookContext(DbContextOptions options) : base(options){ }
    
    public DbSet<Book> Books { get; init; }
    public DbSet<Author> Authors { get; init; }
    public DbSet<Genre.Data.Genre> Genres { get; init; }
    public DbSet<Publisher.Data.Publisher> Publishers { get; init; }
    public DbSet<BookRating> BooksRating { get; init; }
    public DbSet<BookAmount> BooksAmount { get; init; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<BookGenre>();

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(g => g.Books)
            .UsingEntity<BookAuthor>();

        modelBuilder.Entity<BookRating>()
            .HasOne(r => r.Book);
        modelBuilder.Entity<BookRating>()
            .HasKey(r => r.BookId);

        modelBuilder.Entity<BookAmount>()
            .HasOne(a => a.Book);
        modelBuilder.Entity<BookAmount>()
            .HasKey(a => a.BookId);

        modelBuilder.Entity<BookCover>()
            .HasOne(a => a.Book);
        modelBuilder.Entity<BookCover>()
            .HasKey(a => a.BookId);
    }
}