using Microsoft.AspNetCore.Mvc;
using MyBooks.Data.Services;
using MyBooks.Data.ViewModels;

namespace MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _booksService;

        public BooksController(BookService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("allbooks")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _booksService.GetAllBooks();
            return Ok(allBooks);
        }
        [HttpGet("bookbyid")]
        public IActionResult GetBookById(int id)
        {
            var book = _booksService.GetBookById(id);
            return Ok(book);
        }

        [HttpPost("add-book-with-authors")]
        public IActionResult AddBook([FromBody] BookVm book)
        {
            _booksService.AddBookWithAuthors(book);
            return Ok();
        }
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] BookVm book)
        {
            var updateBook = _booksService.UpdateBookById(id, book);
            return Ok(updateBook);
        }
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            _booksService.DeleteBookById(id);
            return Ok();
        }
    }
}
