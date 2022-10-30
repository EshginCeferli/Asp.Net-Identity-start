using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.BlogViewModels
{
    public class BlogCreateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime Date { get; set; }
    }
}
