namespace PetStoreWorkshop.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Store : BaseDeletableModel<string>
{
    public Store()
    {
        this.Id = Guid.NewGuid().ToString();

        this.Pets = new HashSet<Pet>();
        this.Products = new HashSet<Product>();
        this.Services = new HashSet<Service>();
    }

    /// <summary>
    /// Gets or sets store name.
    /// </summary>
    [Comment("Store name")]
    [Required]
    [StringLength(ModelConstants.StoreNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets foreign key to an address.
    /// </summary>
    [Comment("Foreign key to an address")]
    [Required]
    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; }

    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets store description.
    /// </summary>
    [Comment("Store description")]
    [Required]
    [StringLength(ModelConstants.StoreDescriptionLength)]
    public string Description { get; set; }

    public virtual ICollection<Pet> Pets { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}
