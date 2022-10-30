using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.BlogHeaderViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("adminarea")]
    public class BlogHeaderController : Controller
    {     
        private readonly AppDbContext _context;     

        public BlogHeaderController(AppDbContext context)
        {
            _context = context;           
        }
        public async Task<IActionResult>  Index()
        {
            IEnumerable<BlogHeader> blogHeaders = await _context.BlogHeaders.ToListAsync();
            return View(blogHeaders);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogHeaderCreateVM blogHeaderCreateVM)
        {

            if (!ModelState.IsValid)
            {
                return View(blogHeaderCreateVM);
            }


            BlogHeader blogHeader = new BlogHeader()
            {
                Title = blogHeaderCreateVM.Title,
                Description = blogHeaderCreateVM.Description,               
            };

            await _context.BlogHeaders.AddAsync(blogHeader);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            BlogHeader dbBlogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);

            return View(new BlogHeaderEditVM
            {
                Title = dbBlogHeader.Title,
                Description = dbBlogHeader.Description,                
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogHeaderEditVM updatedBlogHeader)
        {
            BlogHeader dbBlogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);
                       

            dbBlogHeader.Title = updatedBlogHeader.Title;
            dbBlogHeader.Description = updatedBlogHeader.Description;
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            BlogHeader blogHeader = await _context.BlogHeaders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (blogHeader == null) return NotFound();
                       
            blogHeader.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            BlogHeader blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);

            BlogHeaderDetailVM blogHeaderDetail = new BlogHeaderDetailVM()
            {
                Title = blogHeader.Title,
                Description = blogHeader.Description,
               
            };

            return View(blogHeaderDetail);
        }
    }
}
