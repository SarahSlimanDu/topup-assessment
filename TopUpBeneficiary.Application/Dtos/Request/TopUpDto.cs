
using System.ComponentModel.DataAnnotations;

namespace TopUpBeneficiary.Application.Dtos.Request
{
    public sealed record TopUpDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid BeneficiaryId { get; set; }
        [Required]
        public Guid topUpOptionId {  get; set; }
    }
}
