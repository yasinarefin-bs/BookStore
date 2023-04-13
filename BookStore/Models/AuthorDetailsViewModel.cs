namespace BookStore.Models
{
    public class AuthorDetailsViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Book> ListOfBooks { get; set; }
    }
}
