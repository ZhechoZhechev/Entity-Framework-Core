namespace PetStoreWorkshop.Data.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Enums;

public class Order : BaseDeletableModel<string>
{
    public Order()
    {
        this.Id = Guid.NewGuid().ToString();

        this.Products = new HashSet<Product>();
        this.Services = new HashSet<Service>();
    }

    /// <summary>
    /// Gets or sets foreign key to a client.
    /// </summary>
    [Comment("Foreign key to a client")]
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; }

    public virtual Client Client { get; set; }

    /// <summary>
    /// Gets or sets the date of the order.
    /// </summary>
    [Comment("Date of the order")]
    [Required]
    public DateTime DateOfOrder { get; set; }

    /// <summary>
    /// Gets or sets the type of delivery.
    /// </summary>
    [Comment("Type of delivery")]
    [Required]
    public DeliveryType DeliveryType { get; set; }

    /// <summary>
    /// Gets or sets the total price of all orders.
    /// </summary>
    [Comment("Total price of all orders")]
    [Required]
    public decimal TotalPrice { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}