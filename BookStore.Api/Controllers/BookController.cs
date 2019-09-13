using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Contexts;
using BookStore.Api.Models;
using BookStore.Api.RequestResponse.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/book
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var result = await _context.Books.ToListAsync();
            return Ok(result);
        }

        // GET api/book/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllBooks(long id)
        {
            var result = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> CreateABook([FromBody] BookRequestModel request)
        {
            var userId = User.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();

            Book aBook = new Book()
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                ApplicationUserId = userId,
                IsApproved = false
            };

            await _context.Books.AddAsync(aBook);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(aBook);
            }

            return BadRequest();
        }
    }
}