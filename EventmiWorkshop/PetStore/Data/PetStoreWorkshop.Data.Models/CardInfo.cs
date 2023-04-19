namespace PetStoreWorkshop.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class CardInfo : BaseDeletableModel<string>
{
    public CardInfo()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets or sets clients payment card number.
    /// </summary>
    [Comment("Card number")]
    [Required]
    [StringLength(ModelConstants.CardNumberLength)]
    public string CardNumber { get; set; }

    /// <summary>
    /// Gets or sets clients payment card expiration date.
    /// </summary>
    [Comment("Card expiration date")]
    [Required]
    [StringLength(ModelConstants.CardExpirationDateLength)]
    public string ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets clients payment card owner name.
    /// </summary>
    [Comment("Card owner name")]
    [Required]
    [StringLength(ModelConstants.CardOwnerNameLength)]
    public string CardHolder { get; set; }

    /// <summary>
    /// Gets or sets clients payment card security number.
    /// </summary>
    [Comment("Card security number")]
    [Required]
    [StringLength(ModelConstants.CardSecurityNumberLength)]
    public string CVC { get; set; }

    /// <summary>
    /// Gets or sets foreign key to a client.
    /// </summary>
    [Comment("Foreign key to a client")]
    [Required]
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; }

    public Client Client { get; set; }
}