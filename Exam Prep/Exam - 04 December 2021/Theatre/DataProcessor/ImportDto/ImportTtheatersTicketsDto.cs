namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    internal class ImportTheatersTicketsDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; }

        public TicketDto[] Tickets { get; set; }
    }

    public class TicketDto 
    {
        [Required]
        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}
