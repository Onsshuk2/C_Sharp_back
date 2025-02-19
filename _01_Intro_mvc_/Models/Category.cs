using System.ComponentModel.DataAnnotations;

namespace _01_Intro_mvc_.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Поле 'Name' є обов'язковим")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина імені – 100 символів")]
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
