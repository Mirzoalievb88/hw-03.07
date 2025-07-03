namespace WebApi.DTOs;

public class ProductCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
}
