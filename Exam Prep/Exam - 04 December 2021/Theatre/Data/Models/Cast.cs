namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter  { get; set; }

        [Required]
        public string PhoneNumber  { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId  { get; set; }
        public Play Play { get; set; }
    }
}