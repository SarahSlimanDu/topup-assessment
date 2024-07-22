

namespace TopUpBeneficiary.Application.Dtos.Request
{
    public sealed record DebitBalanceDto(Guid accountId, decimal debitAmount);
    
}
