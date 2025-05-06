using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;
using SerivicesContracts;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums;
using Services;
using Xunit.Abstractions;
using AutoFixture;
using Microsoft.Extensions.Options;
using FluentAssertions;
using Moq;
using Repository;
using RepositoryContract;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using ContactManager.CORE.Services.PersonServices;
using ContactManager.CORE.Servicies.PersonServices;
using ContactManager.CORE.Services.PersonsServices;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;

namespace ContactManagerTests
{
    public class PersonserviceTests
    {
        private readonly IPersonGetter _personsGetterService;
        private readonly IPersonAdder _personsAdderService;
        private readonly IPersonUpdater _personsUpdaterService;
        private readonly IPersonDeleter _personsDeleterService;
        private readonly IPersonSorter _personsSorterService;

        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<ICurrentApplicationUserService> _CurrentApplicationUserServiceMock;

        private readonly IPersonRepository _personRepository;
        private readonly ICurrentApplicationUserService _currentApplicationUserService;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonserviceTests(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _personRepositoryMock = new Mock<IPersonRepository>();
            _CurrentApplicationUserServiceMock = new Mock<ICurrentApplicationUserService>();
            _personRepository = _personRepositoryMock.Object;
            _currentApplicationUserService = _CurrentApplicationUserServiceMock.Object;
            var loggerMock = new Mock<ILogger<PersonGetter>>();

            _personsGetterService = new PersonGetter(_personRepository,_currentApplicationUserService);

            _personsAdderService = new PersonAdder(_personRepository,_currentApplicationUserService);

            _personsDeleterService = new PersonDeleter(_personRepository);

            _personsSorterService = new PersonSorter(_personRepository);

            _personsUpdaterService = new PersonUpdater(_personRepository);

            _testOutputHelper = testOutputHelper;
        }


        #region AddPersonTests
        //if PersonAddDTO is null ,it should Throw NullException
        [Fact]
        public async Task AddPerson_PersonAddDTO_IsNull()
        {
            //arrange
            PersonAddDTO newPerson = null;
            Person person = null;
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //act
                _personRepositoryMock.Setup(p=>p.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
                await _personsAdderService.AddPerson(newPerson);
            });
        }
        //if PersonName is null, it should Throw argumentExcepion
        [Fact]
        public async Task AddPerson_PersonName_IsNull()
        {
            //arrange
            PersonAddDTO newPerson = new PersonAddDTO
            {
                PersonName = null
            };
            //assert
            _personRepositoryMock.Setup(p => p.AddPerson(It.IsAny<Person>())).ReturnsAsync(null as Person);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //act
                await _personsAdderService.AddPerson(newPerson);
            });
        }
        //if PersonAddDTO is valid,it should add person to list and give him personId and Return valid PersonResponseDTO
        [Fact]
        public async Task AddPerson_PersonAddDTO_Isvalid()
        {
            //arrange
            var newPerson = _fixture.Build<PersonAddDTO>().With(p=> p.Email ,"Asgasd@gmail.com").With(p=>p.phoneNumber,"12968545687").Create();
            Person person = newPerson.ToPerson();
            //act
            _personRepositoryMock.Setup(p => p.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            PersonResponseDTO personResponse =await _personsAdderService.AddPerson(newPerson);
            person.PersonID = personResponse.PersonID;
            //assert

            //Assert.Equal(newPerson.PersonName, personResponse.PersonName);
            personResponse.Should().BeEquivalentTo(person.ToPersonResponseDTO());
        }
        #endregion

        #region GetAllPersonsTests
        //if GetAllPersons is in first call, it should return Empty list
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //act
             List<Person> repoPersons= new List<Person>();
            _personRepositoryMock.Setup(p=>p.GetAllPersons()).ReturnsAsync(repoPersons);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(Guid.NewGuid());
            List<PersonResponseDTO> persons =await _personsGetterService.GetAllPersons();
            //assert
            //Assert.Empty(persons);
            persons.Should().BeEmpty();
        }
        //if GetAllPersons have values ,It should return List of PersonResponseDTO
        [Fact]
        public async Task GetAllPersons_IsValid()
        {
            Guid currentUser = Guid.NewGuid();
            Person person1= _fixture.Build<Person>().With(p => p.Country, null as Country)
                .With(p => p.ApplicationUser, null as ApplicationUser).With(p=>p.ApplicationUserID,currentUser).Create();
           Person person2 = _fixture.Build<Person>().With(p => p.Country, null as Country)
               .With(p => p.ApplicationUser, null as ApplicationUser).With(p => p.ApplicationUserID, currentUser).Create();
           Person person3 = _fixture.Build<Person>().With(p => p.Country, null as Country)
               .With(p => p.ApplicationUser, null as ApplicationUser).With(p => p.ApplicationUserID, currentUser).Create();
            //Act
            List<Person> repoPersons = new List<Person>() { person1,person2,person3};
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(repoPersons);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(currentUser);
            List<PersonResponseDTO> persons =await _personsGetterService.GetAllPersons();
            //assert
            //Assert.NotEmpty(persons);
            persons.Should().NotBeEmpty();
            //Assert.Equal(personResponse, persons.FirstOrDefault());
            persons.Should().BeEquivalentTo(repoPersons.Select(p=>p.ToPersonResponseDTO()).ToList());
        }
        #endregion

        #region GetPersonByIdTests

        //if PersonId is null ,It should throw argumentNullException
        [Fact]
        public async Task GetPersonById_Id_IsNull()
        {
            //arrange
            Guid? personId = null;

            //assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //act
            //    PersonResponseDTO foundPerson =await _personService.GetPersonById(personId);
            //});
            Func<Task> act = async () => await _personsGetterService.GetPersonById(personId);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
        //if PersonId not exist,It should return null
        [Fact]
        public async void GetPersonById_NotFound()
        {
            //arrange
            Guid personId = Guid.NewGuid();
            //act
            _personRepositoryMock.Setup(p => p.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            Func<Task> foundPerson =async ()=>await _personsGetterService.GetPersonById(personId);
            //assert
          await  foundPerson.Should().ThrowAsync<ArgumentException>();
            //Assert.Null(foundPerson);
            //Assert.ThrowsAsync<ArgumentNullException>(async () => await _personService.GetPersonById(personId));

        }
        //if personId exists,It should find the person and return it in PersonResponseDTO
        [Fact]
        public async Task GetPersonById_Found()
        {
            //arrange
            Guid AppUserId = Guid.NewGuid();
            Person person = _fixture.Build<Person>()
             .With(temp=>temp.PersonID,Guid.NewGuid())
             .With(temp => temp.PersonName, "Rahman")
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.ApplicationUserID, AppUserId)
                .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .Create();
            //act
            _personRepositoryMock.Setup(p => p.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(AppUserId);
            PersonResponseDTO personFound =await _personsGetterService.GetPersonById(person.PersonID);
            //assert
            //Assert.NotNull(personFound);
            personFound.Should().NotBeNull();
            //Assert.Equal(personAdded, personFound);
            personFound.Should().BeEquivalentTo(person.ToPersonResponseDTO());
        }
        #endregion


        #region GetFilteredPersonsTests

        //if filterstring is null and filterby Name , it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptyFilterString()
        {
            //arrange

            //arrange
            Guid AppUserId = Guid.NewGuid();
            Person person_request_1 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "Rahman")
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .Create();

            Person person_request_2 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "mary")
             .With(temp => temp.Email, "someone_2@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            Person person_request_3 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "scott")
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();


            List<Person> listOfPersonsToAdd = new List<Person>() { person_request_1, person_request_2, person_request_3 };

           
            //act
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(listOfPersonsToAdd);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(AppUserId);
            List<PersonResponseDTO> personFilterResponse =await _personsGetterService.GetFilteredPersons("PersonName", null);
            //assert
            //Assert.NotNull(personFilterResponse);
            personFilterResponse.Should().NotBeNull();
            //foreach (PersonResponseDTO getAllPerson in getAllPersonsResponse)
            //{
            //    Assert.Contains(getAllPerson, personFilterResponse);
            //}
            personFilterResponse.Should().BeEquivalentTo(listOfPersonsToAdd.Select(p=>p.ToPersonResponseDTO()).ToList());
        }
        //if filter by PersonName & filterstring is valid ,it should search for persons with that Name and return it in list of PersonResponseDto
        [Fact]
        public async Task GetFilteredPerseons_validFilterString()
        {
            //arrange
            Guid AppUserId = Guid.NewGuid();
            Person person_request_1 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "Rahman")
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country,null as Country)
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .Create();

            Person person_request_2 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "mary")
             .With(temp => temp.Email, "someone_2@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            Person person_request_3 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "scott")
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            

            //act
            _personRepositoryMock.Setup(p=>p.GetAllPersons()).ReturnsAsync(new List<Person>() { person_request_1, person_request_2, person_request_3 });
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(AppUserId);
            List<PersonResponseDTO> personFilterResponse =await _personsGetterService.GetFilteredPersons("PersonName", "Ah");
            List<PersonResponseDTO> getAllPersons = new List<PersonResponseDTO>() { person_request_1.ToPersonResponseDTO(), person_request_2.ToPersonResponseDTO(), person_request_3.ToPersonResponseDTO() };
            List<PersonResponseDTO> getAllPersonsWithFilter = getAllPersons.Where(p => p.PersonName.Contains("Ah", StringComparison.OrdinalIgnoreCase)).ToList();
            //assert
            //Assert.NotNull(personFilterResponse);
            personFilterResponse.Should().NotBeNull();
            //foreach (PersonResponseDTO personInGetAll in getAllPersonsWithFilter)
            //{
            //    Assert.Contains(personInGetAll, personFilterResponse);
            //}
            personFilterResponse.Should().BeEquivalentTo(getAllPersonsWithFilter);
        }

        #endregion

        #region GetSortedPersons

        //if sortby is Empty,it should return the unsorted list
        [Fact]
        public async Task GetSortedPersons_Empty_SortedBy()
        {
           
            //arrange
            Guid AppUserId = Guid.NewGuid();
            Person person_request_1 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "Rahman")
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .Create();

            Person person_request_2 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "mary")
             .With(temp => temp.Email, "someone_2@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            Person person_request_3 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "scott")
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();


            List<Person> listOfPersonsToAdd = new List<Person>() { person_request_1, person_request_2, person_request_3 };
            List<PersonResponseDTO> listOfPersonsDtoToAdd=listOfPersonsToAdd.Select(p=>p.ToPersonResponseDTO()).ToList();

            //act
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(listOfPersonsToAdd);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(AppUserId);
            List<PersonResponseDTO> getSortedPersons = _personsSorterService.GetSortedPersons(listOfPersonsDtoToAdd, "", SortOrderEnum.Ascending);
            //assert
            //Assert.Equal(getAllPersonsUnSorted[0], getSortedPersons[0]);
            getSortedPersons.Should().BeSameAs(listOfPersonsDtoToAdd);
        }
        //if sortby is valid  Ex:personName and ascendient,it should sort the unsorted list and return sorted list by personName and ascendient
        [Fact]
        public async Task GetSortedPersons_ValidSort()
        {
            //arrange

            Guid AppUserId = Guid.NewGuid();
            Person person_request_1 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "Rahman")
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .Create();

            Person person_request_2 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "mary")
             .With(temp => temp.Email, "someone_2@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            Person person_request_3 = _fixture.Build<Person>()
             .With(temp => temp.PersonName, "scott")
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.ApplicationUserID, AppUserId)
             .With(temp => temp.ApplicationUser, null as ApplicationUser)
             .With(temp => temp.Country, null as Country)

             .Create();

            List<Person> listOfPersonsToAdd = new List<Person>() { person_request_1, person_request_2, person_request_3 };
            List<PersonResponseDTO> listOfPersonsDtoToAdd = listOfPersonsToAdd.Select(p => p.ToPersonResponseDTO()).ToList();

            //act
            _personRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(listOfPersonsToAdd);
            _CurrentApplicationUserServiceMock.Setup(p => p.currentApplicationUserId).Returns(AppUserId);
            List<PersonResponseDTO> getSortedPersons = _personsSorterService.GetSortedPersons(listOfPersonsDtoToAdd, nameof(PersonResponseDTO.PersonName), SortOrderEnum.Ascending);
            
            getSortedPersons.Should().BeInAscendingOrder(p => p.PersonName);
        }
        #endregion


        #region UpdatePersonTests

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateDTO? person_update_request = null;
            //act
           Func<Task> UpdatePerson = async ()=>await _personsUpdaterService.UpdatePerson(person_update_request);
            //Assert

            //await Assert.ThrowsAsync<ArgumentNullException>(async() => {
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});
            await UpdatePerson.Should().ThrowAsync<ArgumentNullException>();
        }


        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateDTO? person_update_request = new PersonUpdateDTO() { PersonID = Guid.NewGuid() };
            //Act
            Func<Task> UpdatePerson=async ()=> await _personsUpdaterService.UpdatePerson(person_update_request);
            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async() => {
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});
            await UpdatePerson.Should().ThrowAsync<ArgumentException>();
        }


        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            PersonUpdateDTO person_Update = _fixture.Build<PersonUpdateDTO>()
              .With(temp => temp.PersonName, null as string)
              .With(temp => temp.Email, "someone_1@example.com")
              .Create();
            Person person_request_1 = person_Update.ToPerson();

            //Act
            _personRepositoryMock.Setup(p => p.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person_request_1);
            Func<Task> UpdatePerson=async ()=> await _personsUpdaterService.UpdatePerson(person_Update);

            ////Assert
            //await Assert.ThrowsAsync<ArgumentException>(async() => {
            //    //Act
            //    await _personService.UpdatePerson(person_update_request);
            //});
            await UpdatePerson.Should().ThrowAsync<ArgumentException>();

        }


        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            Guid AppUserId = Guid.NewGuid();
            PersonUpdateDTO person_Update = _fixture.Build<PersonUpdateDTO>()
              .With(temp => temp.PersonName, "Rahman")
              .With(temp => temp.Email, "someone_1@example.com")
              .With(temp=>temp.phoneNumber,"01247685467")
              .Create();
            Person person_request_1=person_Update.ToPerson();
           
            //Act
            _personRepositoryMock.Setup(p => p.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person_request_1);
            PersonResponseDTO person_response_from_update =await _personsUpdaterService.UpdatePerson(person_Update);
            person_request_1.PersonID = person_response_from_update.PersonID;

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().BeEquivalentTo(person_request_1.ToPersonResponseDTO());

        }

        #endregion

        #region DeletePersonTests
        //if personId is null or empty ,it should return argumentNullException
        [Fact]
        public async Task DeletePerson_PersonId_IsNull()
        {
            var newPerson = _fixture.Build<PersonAddDTO>().With(p => p.Email, "Asgasd@gmail.com").With(p => p.phoneNumber, "12968545687").Create();

            Person person = _fixture.Build<Person>().With(p => p.Country, null as Country).With(p=>p.ApplicationUser,null as ApplicationUser).Create();
            Guid? personId = null;
            //act
            _personRepositoryMock.Setup(p => p.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);
            _personRepositoryMock.Setup(p => p.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            Func<Task> DeletePerson= async () =>
            await _personsDeleterService.DeletePerson(personId);
            ////assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async ()=>
            ////act
            //await _personService.DeletePerson(person_id)
            //);
            await DeletePerson.Should().ThrowAsync<ArgumentNullException>();
        }
        //if personId doesnt exist ,it should return false
        [Fact]
        public async Task DeletePerson_PersonId_NotFound()
        {
            var newPerson = _fixture.Build<PersonAddDTO>().With(p => p.Email, "Asgasd@gmail.com").With(p => p.phoneNumber, "12968545687").Create();

            Person person = _fixture.Build<Person>().With(p => p.Country, null as Country).With(p=>p.ApplicationUser,null as ApplicationUser).Create();
            Guid personId = Guid.NewGuid();
            //act
            _personRepositoryMock.Setup(p => p.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);
            _personRepositoryMock.Setup(p => p.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            bool isDeleted =await _personsDeleterService.DeletePerson(personId);
            //assert
            //Assert.False( isDeleted );
            isDeleted.Should().BeFalse();
        }
        //if personId exists ,it should find the person with that id and remove it and return true
        [Fact]
        public async Task DeletePerson_PersonId_IsFound()
        {
            //arrange
            Person person = _fixture.Build<Person>().With(p=>p.Country,null as Country).With(p=>p.ApplicationUser,null as ApplicationUser).Create(); 
            Guid personId = Guid.NewGuid();
            //act
            _personRepositoryMock.Setup(p=>p.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);
            _personRepositoryMock.Setup(p => p.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            bool isDeleted =await _personsDeleterService.DeletePerson(personId);
           
            //assert
            //Assert.True(isDeleted);
            //Assert.DoesNotContain(AddPersonResponse, GetAllResult);
            isDeleted.Should().BeTrue();
            
        }
        #endregion

    }
}