using MyBooks.Data.Models;
using MyBooks.Data.ViewModels;

namespace MyBooks.Data.Services
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }
        public void AddBookWithAuthors(BookVm book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book?.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate : 0,
                Genre = book.Genre,
                CoverUrl = book?.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context?.SaveChanges();
            foreach (var id in book.AuthorIds)
            {
                var book_author = new BookAuthor()
                {
                    BookId = _book.Id,
                    AuthorId = id,
                };
                _context?.Book_Authors.Add(book_author);
                _context?.SaveChanges();
            }
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();
        public BookWithAuthorsVm GetBookById(int bookId)
        {
            var _bookWithOuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVm()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead : null,
                Rate = book.IsRead ? book.Rate : 0,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(a => a.Author.FullName).ToList()
            }).FirstOrDefault();
            return _bookWithOuthors;
        }

        public Book UpdateBookById(int bookId, BookVm book)
        {
            var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate : 0;
                _book.Genre = book?.Genre;
                _book.CoverUrl = book?.CoverUrl;
                _context.SaveChanges();
            }
            return _book;
        }
        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);

            if (_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}
