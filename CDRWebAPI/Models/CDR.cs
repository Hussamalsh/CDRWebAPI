using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDRWebAPI.Models;

public class CDR
{

    [Key]
    [Required]
    public string Reference { get; set; } // Unique reference for the call

    [Required]
    public string CallerId { get; set; } //Phone number of the caller

    [Required]
    public string Recipient { get; set; } // Phone number of the number dialled

    [Required]
    public DateTime CallDate { get; set; } // Date on which the call was made

    [Required]
    public TimeSpan EndTime { get; set; } // Time when the call ended

    [Required]
    [Range(0, int.MaxValue)]
    public int Duration { get; set; } // Duration of the call in seconds

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,3)")]
    [Range(0.0, double.MaxValue)]
    public decimal Cost { get; set; } // The billable cost of the call with precision to 3 decimal places

    [Required]
    public string Currency { get; set; } // Currency for the cost - ISO alpha-3
}