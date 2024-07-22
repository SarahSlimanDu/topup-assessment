using FluentAssertions;
using MapsterMapper;
using Moq;
using TopUpBeneficiary.Application.Dtos.Response;
using TopUpBeneficiary.Application.Services.Beneficiaries;
using TopUpBeneficiary.Domain.BeneficiaryAggregate;
using TopUpBeneficiary.Domain.Errors;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Commons;
using TopUpBeneficiary.Domain.Persistence.Interfaces.Repository;
using TopUpBeneficiary.Domain.UserAggregate.ValueObjects;
using TopUpBeneficiary.Domain.UserAggregate;

namespace Services.UnitTests.Beneficiaries
{
    public class GetBeneficiaryTests
    {
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BeneficiaryService _beneficiaryService;

        public GetBeneficiaryTests()
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
        public async Task GetBeneficiaries_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<UserId>())).ReturnsAsync((User)null);

            // Act
            var result = await _beneficiaryService.GetBeneficiaries(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundById());
        }

        [Fact]
        public async Task GetBeneficiaries_ShouldReturnEmptyList_WhenUserHasNoBeneficiaries()
        {
            // Arrange
            var user = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<UserId>())).ReturnsAsync(user);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(It.IsAny<UserId>())).ReturnsAsync(new List<Beneficiary>());

            // Act
            var result = await _beneficiaryService.GetBeneficiaries(user.Id.Value);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task GetBeneficiaries_ShouldReturnBeneficiaries_WhenUserHasBeneficiaries()
        {
            // Arrange
            var user = User.Create("test@test.com", true, AccountId.Create(Guid.NewGuid()));
            var beneficiaries = new List<Beneficiary>
        {
            Beneficiary.Create(user.Id,"1234567890", "Nick1" ),
             Beneficiary.Create(user.Id, "0987654321", "Nick2" )
        };
            var beneficiaryDtos = new List<BeneficiaryDto>
        {
            new BeneficiaryDto { BeneficiaryId = beneficiaries[0].Id.Value, PhoneNumber = beneficiaries[0].PhoneNumber, NickName = beneficiaries[0].NickName },
            new BeneficiaryDto { BeneficiaryId = beneficiaries[1].Id.Value, PhoneNumber = beneficiaries[1].PhoneNumber, NickName = beneficiaries[1].NickName }
        };

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<UserId>())).ReturnsAsync(user);
            _beneficiaryRepositoryMock.Setup(repo => repo.GetActiveBeneficiariesByUserId(It.IsAny<UserId>())).ReturnsAsync(beneficiaries);
            _mapperMock.Setup(mapper => mapper.Map<IList<BeneficiaryDto>>(It.IsAny<IList<Beneficiary>>())).Returns(beneficiaryDtos);

            // Act
            var result = await _beneficiaryService.GetBeneficiaries(user.Id.Value);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(2);
            result.Value.Should().BeEquivalentTo(beneficiaryDtos);
        }
    }
}

