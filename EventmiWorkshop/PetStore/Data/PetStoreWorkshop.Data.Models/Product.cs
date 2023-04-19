namespace PetStoreWorkshop.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Product : BaseDeletableModel<string>
{
    public Product()
    {
        this.Id = Guid.NewGuid().ToString();

        this.Stores = new HashSet<Store>();
        this.Orders = new HashSet<Order>();
    }

    /// <summary>
    /// Gets or sets product name.
    /// </summary>
    [Comment("Product name")]
    [Required]
    [StringLength(ModelConstants.ProductNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets product price.
    /// </summary>
    [Comment("Product price")]
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets foreign key to a category.
    /// </summary>
    [Comment("Foreign key to a category")]
    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
