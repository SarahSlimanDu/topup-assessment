using MapsterMapper;
using Moq;
using TopUpBeneficiary.Application.Dtos.Request;
using TopUpBeneficiary.Application.Services.Beneficiaries;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Domain.UserAggregate;
using FluentAssertions;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;

namespace Services.UnitTests.Beneficiaries
{
    public class AddBeneficiaryTests
    {
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BeneficiaryService _beneficiaryService;

        public AddBeneficiaryTests()
        {
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _beneficiaryService = new BeneficiaryService(
                _beneficiaryRepositoryMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenUserHasFiveActiveBeneficiaries()
        {
            // Arrange
            var user = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));
            var request = new AddBeneficiaryDto { UserId = user.Id.Value, PhoneNumber = "1234567890", NickName = "TestNick" };
            var beneficiaries = new List<Beneficiary>
            {
             Beneficiary.Create( user.Id, "123", "Nick1" ),
             Beneficiary.Create( user.Id, "124", "Nick2"  ),
             Beneficiary.Create( user.Id, "125", "Nick3" ),
             Beneficiary.Create( user.Id, "126", "Nick4" ),
             Beneficiary.Create( user.Id, "127", "Nick5" )
            };

            _userRepositoryMock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(user.Id)).ReturnsAsync(beneficiaries);

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.CountLimitReached());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var request = new AddBeneficiaryDto { UserId = userId.Value, PhoneNumber = "1234567890", NickName = "TestNick" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundById());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenDuplicateNickName()
        {
            // Arrange
            var user = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));
            var request = new AddBeneficiaryDto { UserId = user.Id.Value, PhoneNumber = "1234567890", NickName = "DuplicateNick" };

            var beneficiaries = new List<Beneficiary>
            {
             Beneficiary.Create(user.Id, "123",  "DuplicateNick")
            };

            _userRepositoryMock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(user.Id)).ReturnsAsync(beneficiaries);

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.DuplicateNickName());
        }

        [Fact]
        public async Task AddBeneficiary_ShouldReturnError_WhenDuplicatePhoneNumber()
        {
            // Arrange
            var user = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));
            var request = new AddBeneficiaryDto { UserId = user.Id.Value, PhoneNumber = "123", NickName = "Nick1" };

            var beneficiaries = new List<Beneficiary>
            {
             Beneficiary.Create(user.Id, "123",  "Nick2")
            };

            _userRepositoryMock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(user.Id)).ReturnsAsync(beneficiaries);

            // Act
            var result = await _beneficiaryService.AddBeneficiary(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BeneficiaryErrors.DuplicatePhoneNumber());
        }
    }
}
