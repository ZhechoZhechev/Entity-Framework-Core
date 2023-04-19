namespace PetStoreWorkshop.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Service : BaseDeletableModel<string>
{
    public Service()
    {
        this.Id = Guid.NewGuid().ToString();

        this.Stores = new HashSet<Store>();
        this.Orders = new HashSet<Order>();
    }

    /// <summary>
    /// Gets or sets service name.
    /// </summary>
    [Comment("Service name")]
    [Required]
    [StringLength(ModelConstants.ServiceNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets description of a service.
    /// </summary>
    [Comment("Detailed description of a service")]
    [Required]
    [StringLength(ModelConstants.ServiceDescriptionLength)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets price of a service.
    /// </summary>
    [Comment("Price of a service")]
    [Required]
    public decimal Price { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}