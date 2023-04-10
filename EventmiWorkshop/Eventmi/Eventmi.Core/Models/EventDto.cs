using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Eventmi.Core.Models;
/// <summary>
/// Събитие за трансфер на данни
/// </summary>
public class EventDto
{
    /// <summary>
    /// Идентификатор за запис
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Име на събитието
    /// </summary>
    [Display(Name = "Име на събитието")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Полето '{0}' е задължително!")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Полето '{0}' трябва да е между {2} и {1} символа")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Начало на събитието
    /// </summary>
    [Display(Name = "Начало на събитието")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Полето '{0}' е задължително!")]
    public DateTime Start { get; set; }

    /// <summary>
    /// Край на събитието
    /// </summary>
    [Display(Name = "Край на събитието")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Полето '{0}' е задължително!")]
    public DateTime End { get; set; }

    /// <summary>
    /// Място на събитието
    /// </summary>
    [Display(Name = "Място на събитието")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Полето '{0}' е задължително!")]
    [StringLength(200, MinimumLength = 4, ErrorMessage = "Полето '{0}' трябва да е между {2} и {1} символа")]
    public string Place { get; set; } = null!;

}
