using BookStore.Models.DbModels;

namespace BookStore.Models.ViewModels
{
    public class AuthorDetailsViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Book> ListOfBooks { get; set; }
    }
}
