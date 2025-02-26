using System.ComponentModel.DataAnnotations;

namespace VIOBANK.API.Contracts.Admin
{
    public class CardAmountUpdateDto
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; } = decimal.Zero;

        [Required(ErrorMessage = "CardNumber is required")]
        [MinLength(16, ErrorMessage = "Card number must be at least 16 digits")]
        public string CardNumber { get; set; } = string.Empty;
    }
}
