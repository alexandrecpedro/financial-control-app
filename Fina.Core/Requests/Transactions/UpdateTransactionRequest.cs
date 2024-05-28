using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Transactions;

public class UpdateTransactionRequest : Request
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Invalid title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Invalid type")]
    public ETransactionType Type { get; set; }

    [Required(ErrorMessage = "Invalid value")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Invalid category")]
    public long CategoryId { get; set; }

    [Required(ErrorMessage = "Invalid date")]
    public DateTime? PaidOrReceivedAt { get; set; }
}
