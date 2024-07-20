

namespace TopUpBeneficiary.Application.Dtos.Request
{
    public class TopUpRequest
    {
        public Guid UserId { get; set; }    
        public Guid BeneficiaryId { get; set; }
        public Guid topUpOptionId {  get; set; }    //TODO: Should be string as best practice
    }
}
