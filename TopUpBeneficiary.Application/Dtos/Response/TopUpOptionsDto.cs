

namespace TopUpBeneficiary.Application.Dtos.Response
{
    public sealed record TopUpOptionsDto
    {
        public Guid Id { get; set; }    
        public int Amount { get;  set; }
        public DateTime CreatedDateTime { get;  set; }
    }
}
