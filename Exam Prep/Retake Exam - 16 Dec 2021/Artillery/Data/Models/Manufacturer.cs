namespace Artillery.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string ManufacturerName  { get; set; }

        [Required]
        [MaxLength(100)]
        public string Founded  { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
