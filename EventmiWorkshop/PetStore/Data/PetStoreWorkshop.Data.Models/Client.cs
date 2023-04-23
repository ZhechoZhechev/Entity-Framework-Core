namespace PetStoreWorkshop.Data.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using PetStoreWorkshop.Data.Models.Common;

public class Client : ApplicationUser
{
    public Client()
    {
        this.PaymentCards = new HashSet<CardInfo>();
        this.Pets = new HashSet<Pet>();
        this.Orders = new HashSet<Order>();
    }
    /// <summary>
    /// Gets or sets client first name.
    /// </summary>
    [Comment("Client first name")]
    [Required]
    [StringLength(ModelConstants.ClientNameLength)]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets client last name.
    /// </summary>
    [Comment("Client last name")]
    [Required]
    [StringLength(ModelConstants.ClientNameLength)]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets foreign key to an address.
    /// </summary>
    [Comment("Foreign key to an address")]
    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; }

    public virtual Address Address { get; set; }

    /// <summary>
    /// Gets or sets foreign key to client card.
    /// </summary>
    [Comment("Foreign key to client card")]
    [ForeignKey(nameof(ClientCard))]
    public string ClientCardId { get; set; }

    public virtual ClientCard ClientCard { get; set; }

    public virtual ICollection<CardInfo> PaymentCards { get; set; }

    public virtual ICollection<Pet> Pets { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
