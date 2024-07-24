

namespace TopUpBeneficiary.Application.Dtos.Request
{
    public sealed record DebitBalanceDto(string accountIban, decimal debitAmount);
    
}
