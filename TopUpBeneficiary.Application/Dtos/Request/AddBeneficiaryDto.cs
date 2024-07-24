using System.ComponentModel.DataAnnotations;

namespace TopUpBeneficiary.Application.Dtos.Request
{
    public sealed record AddBeneficiaryDto
    {
        [Required]
        public Guid UserId { get;  set; }

        [Required]
        [MaxLength(10)]
        [Phone]
        public string PhoneNumber { get;  set; }

        [Required]
        [MaxLength(20)]
        public string NickName { get;  set; }
    }
}
