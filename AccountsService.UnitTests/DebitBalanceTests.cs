using Accounts.Application.Dtos.Request;
using Accounts.Domain.AccountAggregate;
using Accounts.Domain.AccountAggregate.ValueObjects;
using Accounts.Domain.Errors;
using Accounts.Domain.TransactionAggregate;


namespace AccountsService.UnitTests
{
    public class DebitBalanceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AccountService _accountService;

        public DebitBalanceTests()
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
        public async Task DebitBalance_ShouldReturnFailure_WhenAccountNotFound()
        {
            // Arrange
            var request = new DebitBalanceRequest { AccountIban = "DE89370400440532013000", DebitAmount = 100 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIban(It.IsAny<string>()))
                .ReturnsAsync((Account)null);

            // Act
            var result = await _accountService.DebitBalance(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountErrors.NotFoundById());
            _transactionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Transaction>()), Times.Never);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Never);
        }

        [Fact]
        public async Task DebitBalance_ShouldReturnFailure_WhenNotEnoughBalance()
        {
            // Arrange
            var account = Account.Create("AED133424", "account@test.com", "P@ssw0rd", 10, "AED");
            var request = new DebitBalanceRequest { AccountIban = account.Iban, DebitAmount = 100 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIban(It.IsAny<string>()))
                .ReturnsAsync(account);

            // Act
            var result = await _accountService.DebitBalance(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountErrors.NoEnoughBalance());
            _transactionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Transaction>()), Times.Never);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Never);
        }

        [Fact]
        public async Task DebitBalance_ShouldReturnSuccess_WhenBalanceIsDebited()
        {
            // Arrange
            var account = Account.Create("AED133424", "account@test.com", "P@ssw0rd", 150, "AED");
            var request = new DebitBalanceRequest { AccountIban = account.Iban, DebitAmount = 100 };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByIban(It.IsAny<string>()))
                .ReturnsAsync(account);
            _transactionRepositoryMock.Setup(repo => repo.Add(It.IsAny<Transaction>())).Verifiable();
            _accountRepositoryMock.Setup(repo => repo.UpdateAccount(It.IsAny<Account>())).Verifiable();
            _unitOfWorkMock.Setup(uow => uow.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _accountService.DebitBalance(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _transactionRepositoryMock.Verify(repo => repo.Add(It.Is<Transaction>(t => t.AccountId == account.Id && t.Amount == request.DebitAmount)), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccount(It.Is<Account>(a => a.Balance == 50)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Once);
        }
    }

 
}
