using _01_Intro_mvc_.data;
using _01_Intro_mvc_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _01_Intro_mvc_.ViewModels;

namespace _01_Intro_mvc_.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsEnumerable();

            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Product = new Product(),
                Categories = categories.Select(c =>
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CreateProductVM viewModel)
        {
            string? imagePath = null;

            if (viewModel.File != null)
            {
                imagePath = SaveImage(viewModel.File);
            }

            viewModel.Product.Image = imagePath;
            viewModel.Product.Id = Guid.NewGuid().ToString();
            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile file)
        {
            var types = file.ContentType.Split("/");
            if (types[0] != "image")
            {
                return null;
            }

            string fileName = $"{Guid.NewGuid()}.{types[1]}";
            string imagesPath = Path.Combine(_environment.WebRootPath, "images", "products");
            string filePath = Path.Combine(imagesPath, fileName);
            using (var stream = file.OpenReadStream())
            {
                using (var fileStream = System.IO.File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        public IActionResult Update(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product model)
        {
            _context.Products.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product model)
        {
            _context.Products.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

