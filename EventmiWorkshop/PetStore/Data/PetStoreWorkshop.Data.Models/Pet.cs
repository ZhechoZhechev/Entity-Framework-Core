namespace PetStoreWorkshop.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Pet : BaseDeletableModel<string>
{
    public Pet()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets or sets pet name if it has one.
    /// </summary>
    [Comment("Pet name if it has one")]
    [StringLength(ModelConstants.PetNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets pet age.
    /// </summary>
    [Comment("Pet age")]
    [Required]
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets pet breed.
    /// </summary>
    [Comment("Pet breed")]
    [Required]
    [StringLength(ModelConstants.PetBreedLength)]
    public string Breed { get; set; }

    /// <summary>
    /// Gets or sets pet price.
    /// </summary>
    [Comment("Pet price")]
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

    /// <summary>
    /// Gets or sets foreign key to a client.
    /// </summary>
    [Comment("Foreign key to a client")]
    [ForeignKey(nameof(Owner))]
    public string ClientId { get; set; }

    public virtual Client Owner { get; set; }

    /// <summary>
    /// Gets or sets foreign key to a store.
    /// </summary>
    [Comment("Foreign key to a store")]
    [Required]
    [ForeignKey(nameof(Store))]
    public string StoreId { get; set; }

    public virtual Store Store { get; set; }
}
