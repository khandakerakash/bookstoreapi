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
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // Get api/category
        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var result = await _context.Categories.ToListAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        // Get api/category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetACategory(long id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // Post api/Category
        [HttpPost]
        public async Task<IActionResult> CreateACategory([FromBody] AddCategoryRequest request)
        {
            Category aCategory = new Category()
            {
                Description = request.Description,
                Name = request.Name
            };
            await _context.Categories.AddAsync(aCategory);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(result);
            }

            return NotFound();
        }

        // Put api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateACategory(long id, [FromBody] Category category)
        {
            //var result = await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();

            //if (result == null)
            //{
            //    return NotFound();
            //}

            //_context.Update(category);

            //if (_context.SaveChanges()!=0)
            //{
            //    return NoContent();
            //}

            //return BadRequest();

            if (category == null || category.CategoryId != id)
            {
                return BadRequest();
            }

            var result = await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            _context.Update(category);
            return new NoContentResult();

        }

        // Delete api/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            if (_context.SaveChanges() == 1)
                return NoContent();

            return BadRequest();

        }















    }
}