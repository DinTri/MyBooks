using MyBooks.Data.Models;
using MyBooks.Data.ViewModels;

namespace MyBooks.Data.Services
{
    public class AuthorsService
    {
        private readonly AppDbContext _context;

        public AuthorsService(AppDbContext context)
        {
            _context = context;
        }

        public void AddAuthor(AuthorVm author)
        {
            var _author = new Author()
            {
                FullName = author.FullName,
            };
            _context.Authors.Add(_author);
            _context?.SaveChanges();
        }

        public AuthorWithBooksVm GetAuthorsWithBooks(int authorId)
        {
            var _author = _context.Authors.Where(n => n.Id == authorId).Select(n => new AuthorWithBooksVm()
            {
                FullName = n.FullName,
                BookTitles = n.Book_Authors.Select(a => a.Book.Title).ToList()
            }).FirstOrDefault();
            return _author;
        }
    }
}
