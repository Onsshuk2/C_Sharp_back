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
            
            _environment = environment;_context = context;
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
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id
                    });

                return View(viewModel);
            }

            string? imagePath = SaveImage(viewModel.File);
            if (imagePath == null)
            {
                ModelState.AddModelError("File", "Помилка завантаження зображення");
                viewModel.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id
                    });

                return View(viewModel);
            }

            viewModel.Product.Image = imagePath;
            viewModel.Product.Id = Guid.NewGuid().ToString();
            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        private string? SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var types = file.ContentType.Split('/');
            if (types[0] != "image")
            {
                return null;
            }

            string fileName = $"{Guid.NewGuid()}.{types[1]}";
            string imagesPath = Path.Combine(_environment.WebRootPath, "images", "products");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            string filePath = Path.Combine(imagesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
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

            var viewModel = new CreateProductVM
            {
                Product = product,
                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromForm] CreateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id
                    });

                return View(viewModel);
            }

            var product = _context.Products.Find(viewModel.Product.Id);
            if (product == null)
                return NotFound();

            product.Name = viewModel.Product.Name;
            product.Description = viewModel.Product.Description;
            product.Price = viewModel.Product.Price;
            product.Amount = viewModel.Product.Amount;
            product.CategoryId = viewModel.Product.CategoryId;

            if (viewModel.File != null)
            {
                string? imagePath = SaveImage(viewModel.File);
                if (imagePath == null)
                {
                    ModelState.AddModelError("File", "Помилка завантаження зображення");
                    viewModel.Categories = _context.Categories
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id
                        });

                    return View(viewModel);
                }
                product.Image = imagePath;
            }

            _context.Products.Update(product);
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

