using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _01_Intro_mvc_.Models
{
    public class Product
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Поле 'Назва' є обов'язковим")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле 'Ціна' є обов'язковим")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більше 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Поле 'Кількість' є обов'язковим")]
        [Range(1, int.MaxValue, ErrorMessage = "Кількість має бути більше 0")]
        public int Amount { get; set; }

        [MaxLength(255)]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Оберіть категорію")]
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
