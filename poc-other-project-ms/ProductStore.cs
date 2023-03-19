using System.ComponentModel.DataAnnotations;

namespace poc_other_project_ms;



public class ProductStore
{
    [Key]
    public int Id { get; set; }
    public string UPC { get; set; }
    public string StoreName { get; set; }
}
