using LibraryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> _books = new()
        {
            new Book { Id = 1, Title = "Suç ve Ceza", Author = "Dostoyevski", Year = 1866 },
            new Book { Id = 2, Title = "Kürk Mantolu Madonna", Author = "Sabahattin Ali", Year = 1943 },
            new Book { Id = 3, Title = "Simyacı", Author = "Paulo Coelho", Year = 1988 }
        };

        /// <summary>
        /// lists all books
        /// </summary>
        /// <response code="200">all books listed</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Book>> GetAll()
        {
            return Ok(_books);
        }

        /// <summary>
        /// gets the book by ID
        /// </summary>
        /// <param name="id">the id of the book</param>
        /// <response code="200">the book was found</response>
        /// <response code="404">no book with the given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// creates book with new id and given data
        /// </summary>
        /// <param name="book">the book to create</param>
        /// <response code="201">the book created</response>
        /// <response code="400">broken json</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Book> Create(Book book)
        {
            book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
            _books.Add(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        /// <summary>
        /// updates book with new datas
        /// </summary>
        /// <param name="id">The id of the book to update</param>
        /// <param name="updatedBook">the new book data</param>
        /// <response code="204">book updated</response>
        /// <response code="400">broken json</response>
        /// <response code="404">no book with the given ID</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return NoContent();
        }

        /// <summary>
        /// deletes book in the given id
        /// </summary>
        /// <param name="id">the id of the book to delete</param>
        /// <response code="204">the book deleted</response>
        /// <response code="404">no book with the given id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if(book == null)
            {
                return NotFound();
            }

            _books.Remove(book);

            return NoContent();
        }

    }
}
