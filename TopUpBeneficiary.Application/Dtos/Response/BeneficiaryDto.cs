
namespace TopUpBeneficiary.Application.Dtos.Response
{
    public sealed record BeneficiaryDto
    {
        public Guid BeneficiaryId { get; set; } 
        public string PhoneNumber { get;  set; }
        public string NickName { get;  set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get;   set; }
    }
}
