using MyBooks.Data.Models;
using MyBooks.Data.ViewModels;

namespace MyBooks.Data.Services
{
    public class BookService
    {
        private readonly AppDbContext? _context;

        public BookService(AppDbContext? context)
        {
            _context = context;
        }
        public void AddBook(BookVm? book)
        {
            var _book = new Book()
            {
                Title = book?.Title,
                Description = book?.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book?.Genre,
                Author = book?.Author,
                CoverUrl = book?.CoverUrl,
                DateAdded = DateTime.Now
            };
            _context.Books.Add(_book);
            _context?.SaveChanges();
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();
        public Book GetBookById(int bookId) => _context.Books.FirstOrDefault(b => b.Id == bookId);
        public Book UpdateBookById(int bookId, BookVm book)
        {
            var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book?.Genre;
                _book.Author = book?.Author;
                _book.CoverUrl = book?.CoverUrl;
                _context.SaveChanges();
            }
            return _book;
        }
    }
}
