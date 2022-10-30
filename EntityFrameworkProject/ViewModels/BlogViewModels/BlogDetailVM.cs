using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.BlogViewModels
{
    public class BlogDetailVM
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime Date { get; set; }
    }
}
