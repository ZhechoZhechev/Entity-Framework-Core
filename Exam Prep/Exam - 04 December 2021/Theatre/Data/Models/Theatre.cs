namespace Theatre.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Sockets;

    public class Theatre
    {
        public Theatre() 
        {
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public sbyte NumberOfHalls  { get; set; }

        [Required]
        [MaxLength(30)]
        public string Director  { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
