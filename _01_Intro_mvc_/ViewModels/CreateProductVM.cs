using _01_Intro_mvc_.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _01_Intro_mvc_.ViewModels
{
    
        public class CreateProductVM
        {
            public Product Product { get; set; } = new();
            public IEnumerable<SelectListItem> Categories { get; set; } = [];
        [Display(Name = "Фото товару")]
        public IFormFile? File { get; set; }
        }
    }

