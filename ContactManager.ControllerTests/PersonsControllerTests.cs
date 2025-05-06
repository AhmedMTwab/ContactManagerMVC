using AutoFixture;
using ContactManager.Controllers;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.ServiciesContracts.IContryServices;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums;

namespace ContactManagerTests
{
    public class PersonsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IPersonGetter _personGetterService;
        private readonly IPersonsFIleGetter _personsFileGetterService;
        private readonly IPersonAdder _personsAdderService;
        private readonly IPersonSorter _personsSorterService;
        private readonly IPersonDeleter _personsDeleterService;
        private readonly IPersonUpdater _personsUpdaterService;
        private readonly ICountryGetter _countryGetterService;

        private readonly ICurrentApplicationUserService _currentApplicationUserService;

        private readonly ILogger<PersonsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly Mock<IPersonGetter> _personGetterServiceMock;
        private readonly Mock<IPersonsFIleGetter> _personsFileGetterServiceMock;
        private readonly Mock<IPersonAdder> _personsAdderServiceMock;
        private readonly Mock<IPersonUpdater> _personsUpdaterServiceMock;
        private readonly Mock<IPersonSorter> _personsSorterServiceMock;
        private readonly Mock<IPersonDeleter> _personsDeleterServiceMock;

        private readonly Mock<ICurrentApplicationUserService> _currentApplicationUserServiceMock;

        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private readonly Mock<ICountryGetter> _countryGetterServiceMock;
        private readonly PersonsController _personsController;
        public PersonsControllerTests()
        {
            _fixture = new Fixture();
            _personGetterServiceMock = new Mock<IPersonGetter>();
            _personsFileGetterServiceMock = new Mock<IPersonsFIleGetter>();
            _personsAdderServiceMock = new Mock<IPersonAdder>();
            _personsDeleterServiceMock = new Mock<IPersonDeleter>();
            _personsUpdaterServiceMock = new Mock<IPersonUpdater>();
            _personsSorterServiceMock = new Mock<IPersonSorter>();

            #region UserManagerMock
            var store = new Mock<IUserStore<ApplicationUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();


            _currentApplicationUserServiceMock = new Mock<ICurrentApplicationUserService>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
               store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators.AsEnumerable(),
                passwordValidators.AsEnumerable(),
                keyNormalizer.Object,
                errors.Object,
                services.Object,
                logger.Object
                ); 
            #endregion

            _loggerMock = new Mock<ILogger<PersonsController>>();


            _countryGetterServiceMock = new Mock<ICountryGetter>();
            _personGetterService = _personGetterServiceMock.Object;
            _personsFileGetterService = _personsFileGetterServiceMock.Object;
            _personsAdderService = _personsAdderServiceMock.Object;
            _personsUpdaterService = _personsUpdaterServiceMock.Object;
            _personsDeleterService = _personsDeleterServiceMock.Object;
            _personsSorterService = _personsSorterServiceMock.Object;

            _currentApplicationUserService = _currentApplicationUserServiceMock.Object;

            _userManager = _userManagerMock.Object;
            _logger = _loggerMock.Object;

            _countryGetterService = _countryGetterServiceMock.Object;
            _personsController = new PersonsController(_personGetterService, _personsAdderService, _personsDeleterService, _personsUpdaterService, _personsSorterService, _countryGetterService, _personsFileGetterService, _logger, _userManager, _currentApplicationUserService);

        }
        [Fact]
        public async Task Index_ShouldReturnViewResult()
        {
            //arrange
            List<PersonResponseDTO> personResponses = _fixture.Create<List<PersonResponseDTO>>();
            //Act
            _personGetterServiceMock.Setup(x => x.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(personResponses);
            _personsSorterServiceMock
            .Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponseDTO>>(), It.IsAny<string>(), It.IsAny<SortOrderEnum>())).Returns(personResponses);

            var IndexResponse = await _personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderEnum>());
            //Assert
            var viewResult = Assert.IsType<ViewResult>(IndexResponse);
            var Model = viewResult.ViewData.Model;
            Model.Should().BeAssignableTo<IEnumerable<PersonResponseDTO>>();
            Model.Should().Be(personResponses);

        }
    }
}
