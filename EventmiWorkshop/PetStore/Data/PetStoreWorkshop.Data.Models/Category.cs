namespace PetStoreWorkshop.Data.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Category : BaseDeletableModel<int>
{
    public Category()
    {
        this.Pets = new HashSet<Pet>();
        this.Products = new HashSet<Product>();
    }

    [Required]
    [MaxLength(ModelConstants.CategoryNameLength)]
    public string Name { get; set; }

    public virtual ICollection<Pet> Pets { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}