using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult>  Index()
        {
            IEnumerable<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }

        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blogCreateVM)
        {

            if (!ModelState.IsValid)
            {
                return View(blogCreateVM);
            }

            if (!blogCreateVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(blogCreateVM);
            }


            if (!blogCreateVM.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(blogCreateVM);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + blogCreateVM.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

            await Helper.SaveFile(path, blogCreateVM.Photo);

            Blog blog = new Blog()
            {
                Title = blogCreateVM.Title,
                Desc = blogCreateVM.Description,
                Date = DateTime.Now,
                Image = fileName                
            };

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Blog dbBlog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

            return View(new BlogEditVM
            {
                Title = dbBlog.Title,
                Description = dbBlog.Desc,
                Image = dbBlog.Image,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditVM updatedBlog)
        {
            Blog dbBlog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

            if (updatedBlog.Photo != null)
            {              

                if (!updatedBlog.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View(updatedBlog);
                }


                if (!updatedBlog.Photo.CheckFileSize(500))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View(updatedBlog);
                }

                //string oldPath = Helper.GetFilePath(_env.WebRootPath, "img", dbBlog.Image);
                //Helper.DeleteFile(oldPath);

                string fileName = Guid.NewGuid().ToString() + "_" + updatedBlog.Photo.FileName;

                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                await Helper.SaveFile(path, updatedBlog.Photo);

                updatedBlog.Image = fileName;

                dbBlog.Image = updatedBlog.Image;
            }

            dbBlog.Title = updatedBlog.Title;
            dbBlog.Desc = updatedBlog.Description;
            dbBlog.Date = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _context.Blogs                             
                .FirstOrDefaultAsync(m=>m.Id == id);

            if (blog == null) return NotFound();

                string path = Helper.GetFilePath(_env.WebRootPath, "img", blog.Image);
                Helper.DeleteFile(path);              
            
            blog.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

            BlogDetailVM blogDetailVM = new BlogDetailVM() { 
            Title = blog.Title,
            Description = blog.Desc,
            Date = blog.Date,
            Image = blog.Image
            };

            return View(blogDetailVM);
        }
    }
}
