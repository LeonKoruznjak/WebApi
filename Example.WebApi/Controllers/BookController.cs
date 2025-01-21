using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book>? _books=new List<Book>();


        [HttpGet]
        public IActionResult GetBooks()
        {
            if(_books==null)
            {
                return NoContent();
                
            }else if (_books.Count() == 0)
            {
                return NotFound("There are no found books");
            }
            else
            {
                return Ok(_books);
            }
          
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(Guid id)
        {
            var book=_books.FirstOrDefault(b=>b.Id.Equals(id));
            if (book == null)
            {
                return NoContent();
            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult AddBook( Book newBook)
        {

          
            if (newBook == null)
            {
                return BadRequest("Book object cannot be null.");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_books.Any(b => b.Id.Equals(newBook.Id)))
            {
                return BadRequest("There already exits a book with the same id");
            }


            _books.Add(newBook);
            return Created();

        }



        // Nova POST ruta koja prima parametre kroz URL
        [HttpPost("add/{title}/{description}/{author}/{quantity}")]
        public IActionResult AddBookWithParams(string title, string description, string author, int quantity)
        {
            var newBook = new Book(title, description, author, quantity);

            
            _books.Add(newBook);

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBook(Guid id,[FromBody] Book updatedBook)
        {


            var book = _books.FirstOrDefault(b => b.Id.Equals(id));
            if (book == null)
            {
                return NotFound("Book isnt found.");
            }

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Author = updatedBook.Author;
            book.Quantity = updatedBook.Quantity;

            return Ok(book);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
         
            var book = _books.FirstOrDefault(b => b.Id.Equals(id));
            if (book == null)
            {
                return NotFound("Book isnt found.");
            }
            _books.Remove(book);
            return Ok();
        }
    }
}
