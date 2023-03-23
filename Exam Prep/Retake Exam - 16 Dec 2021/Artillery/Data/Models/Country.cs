namespace Artillery.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CountryName  { get; set; }

        [Required]
        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns  { get; set; }
    }
}
