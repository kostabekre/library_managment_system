namespace LibraryManagementSystemAPI.Books.Data;

public class BookRating
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Rating { get; set; }
}