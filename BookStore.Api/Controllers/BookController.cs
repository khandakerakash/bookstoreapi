using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Contexts;
using BookStore.Api.Models;
using BookStore.Api.RequestResponse.Request;
using BookStore.Api.Services;
using Hangfire;
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

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "manager")]
        [HttpPost]
        public async Task<ActionResult> CreateABook([FromForm] BookRequestModel request)
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
                CategoryId = request.CategoryId,
                IsApproved = false
            };
            if (request.ImageUrl != null && request.ImageUrl.Length > 0)
            {
                var fileName = Path.GetFileName(request.ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageUrl.CopyToAsync(fileSteam);
                }
                aBook.ImageUrl = fileName;
            }

            

            await _context.Books.AddAsync(aBook);

            if (await _context.SaveChangesAsync() > 0)
            {
               // var adminUser = _
                BackgroundJob.Enqueue<ILoginService>(x => x.SendEMailAdmin());
                var mybook = aBook;
                return Ok(new
                {
                    aBook.Title,
                    aBook.ImageUrl,
                    aBook.BookId,
                    aBook.CategoryId,
                    aBook.Price,
                    aBook.ApplicationUserId,
            
                });
            }
            return BadRequest();
        }
    }
}