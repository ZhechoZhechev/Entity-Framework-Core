namespace PetStoreWorkshop.Data.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PetStoreWorkshop.Data.Common.Models;
using PetStoreWorkshop.Data.Models.Common;

public class Address : BaseDeletableModel<int>
{
    public Address()
    {
        this.Clients = new HashSet<Client>();
    }

    /// <summary>
    /// Gets or sets text of the address.
    /// </summary>
    [Comment("Text of the address")]
    [Required]
    [StringLength(ModelConstants.AddressTextLength)]
    public string AddressText { get; set; }

    /// <summary>
    /// Gets or sets name of the town of the address.
    /// </summary>
    [Comment("Name of the town")]
    [Required]
    [StringLength(ModelConstants.AddressTownNameLength)]
    public string TownName { get; set; }

    public virtual ICollection<Client> Clients { get; set; }
}
