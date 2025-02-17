using System.ComponentModel.DataAnnotations;

namespace _01_Intro_mvc_.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(255)]
        public List<Product> Product { get; set;}=new List<Product>();
    }
}

