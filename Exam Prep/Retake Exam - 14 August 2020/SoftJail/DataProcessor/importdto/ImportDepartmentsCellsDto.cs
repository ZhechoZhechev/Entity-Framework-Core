namespace SoftJail.DataProcessor.ImportDto
{
using System.ComponentModel.DataAnnotations;
    public class ImportDepartmentsCellsDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        public ImportCellDto[] Cells { get; set; }
    }

    public class ImportCellDto 
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        public bool HasWindow { get; set; }
    }
}
