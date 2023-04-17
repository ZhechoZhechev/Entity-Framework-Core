using System.ComponentModel.DataAnnotations;

namespace QuizzWeb.Core.Models;

public class QuizzModel
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "The field is mandatory!")]
    [StringLength(100, MinimumLength = 5)]
    public string Name { get; set; } = null!;
}
