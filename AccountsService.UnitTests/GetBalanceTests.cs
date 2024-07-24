
using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.Errors;

namespace AccountsService.UnitTests
{
    public class GetBalanceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AccountService _accountService;

        public GetBalanceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _accountService = new AccountService(
                _accountRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task GetBalance_ShouldReturnFailure_WhenAccountNotFound()
        {
            // Arrange
            var accountId = "DE89370400440532013000";

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIban(It.IsAny<string>()))
                .ReturnsAsync((Account)null);

            // Act
            var result = await _accountService.GetBalance(accountId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(AccountErrors.NotFoundById());
        }

        [Fact]
        public async Task GetBalance_ShouldReturnSuccess_WithAccountBalance()
        {
            // Arrange
            var account = Account.Create("AED133424", "account@test.com", "P@ssw0rd", 100, "AED"); ;

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIban(It.IsAny<string>()))
                .ReturnsAsync(account);

            // Act
            var result = await _accountService.GetBalance(account.Iban);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Balance.Should().Be(100);
        }
    }
}
