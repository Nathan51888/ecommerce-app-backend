using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Features.Category.Models;

[Table("category")]
public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}