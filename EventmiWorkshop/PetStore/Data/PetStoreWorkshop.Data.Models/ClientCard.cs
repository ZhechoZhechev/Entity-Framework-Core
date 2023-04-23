namespace PetStoreWorkshop.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class ClientCard : BaseDeletableModel<string>
{
    public ClientCard()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets or sets clients client card number.
    /// </summary>
    [Comment("Client card number")]
    [Required]
    [StringLength(ModelConstants.ClientCardNumberLength)]
    public string CardNumber { get; set; }

    /// <summary>
    /// Gets or sets clients client card expiration date.
    /// </summary>
    [Comment("Client card expiration date")]
    [Required]
    [StringLength(ModelConstants.ClientCardExpirationDateLength)]
    public string ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage.
    /// </summary>
    [Comment("Percent discount")]
    [Required]
    public int Discount { get; set; }

    /// <summary>
    /// Gets or sets foreign key to a client.
    /// </summary>
    [Comment("Foreign key to a client")]
    [Required]
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; }

    public virtual Client Client { get; set; }
}