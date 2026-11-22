using LibraryApi.Models;
using LibraryApi.DTOs;
using LibraryApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// lists all books
        /// </summary>
        /// <response code="200">all books listed</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Book>>> GetAll()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);   
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
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// creates book with new id and given data
        /// </summary>
        /// <param name="dto">the book to create</param>
        /// <response code="201">the book created</response>
        /// <response code="400">broken json</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> Create(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Year = dto.Year
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = book.Id}, book);
        }

        /// <summary>
        /// updates book with new datas
        /// </summary>
        /// <param name="id">The id of the book to update</param>
        /// <param name="dto">the new book data</param>
        /// <response code="204">book updated</response>
        /// <response code="400">broken json</response>
        /// <response code="404">no book with the given ID</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Year = dto.Year;

            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
