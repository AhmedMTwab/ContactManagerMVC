using System.Runtime.InteropServices;
using AutoFixture;
using Entities;
using Microsoft.EntityFrameworkCore;
using SerivicesContracts;
using SerivicesContracts.DTO;
using Services;
using FluentAssertions;
using Moq;
using Repository;
using RepositoryContract;
using ContactManager.CORE.ServiciesContracts.IContryServices;
namespace ContactManagerTests
{
    public class CountryServiceTests
    {
        private readonly ICountryAdder _countryAdderService;
        private readonly ICountryGetter _countryGetterService;
        private readonly IFixture _fixture;
        private readonly Mock<ICountryRepository> _CountryRepoMock;
        private readonly ICountryRepository _countryRepository;
        //constructor
        public CountryServiceTests()
        {
            _fixture = new Fixture();

            _CountryRepoMock = new Mock<ICountryRepository>();
            _countryRepository = _CountryRepoMock.Object;
           

            _countryAdderService = new CountryAdder(_countryRepository);
            _countryGetterService = new ConuntryGetter(_countryRepository);
        }

        #region AddCountryTests
        //if the countryAddDto is null, it should throw an exception
        [Fact]
        public async Task AddCountry_CountryAddDto_IsNull_NullException()
        {
            //arange
            CountryAddDTO countryAddDto = null;
            Country country =_fixture.Build<Country>().With(c=>c.Persons,null as List<Person>).Create();
            //act
            _CountryRepoMock.Setup(c=>c.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
            Func<Task> AddCountry=async  () =>await _countryAdderService.AddCountry(countryAddDto);
            //assert
            //await Assert.ThrowsAsync<ArgumentNullException>(
            //    //act
            //    () => _countryService.AddCountry(countryAddDto));
            await AddCountry.Should().ThrowAsync<ArgumentNullException>();
        }
        //if the countryName is empty , it should throw an exception
        [Fact]
        public async Task AddCountry_CountryName_IsNull_ArgumentException()
        {
            //arange
            CountryAddDTO countryAddDto = new CountryAddDTO()
            {
                CountryName = null
            };
            Country country = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            //act
            _CountryRepoMock.Setup(c=>c.GetCountrybyId(It.IsAny<Guid?>())).ReturnsAsync(country);
            _CountryRepoMock.Setup(c => c.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
            Func<Task> AddCountry = () => _countryAdderService.AddCountry(countryAddDto);
            //assert
            await AddCountry.Should().ThrowAsync<ArgumentException>();

        }
        //if the countryAddDto is valid, and there is duplication, it should throw an exception
        [Fact]
        public async Task AddCountry_CountryName_IsExist_ArgumentException()
        {
            //arange
            CountryAddDTO countryAddDto2 = _fixture.Build<CountryAddDTO>().With(c => c.CountryName, "Egypt").Create();
            Country country = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            //act
            _CountryRepoMock.Setup(c => c.GetCountrybyName(It.IsAny<string>())).ReturnsAsync(country);
            Func<Task> responseDto2 = async () => await _countryAdderService.AddCountry(countryAddDto2);
            //assert
            //Assert.Null(responseDto2);
            await responseDto2.Should().ThrowAsync<ArgumentException>();
        }
        //if the countryAddDto is valid, it should return a countryResponse
        [Fact]
        public async Task AddCountry_CountryAddDto_IsValid_CountryResposeDTO()
        {
            //arange
            CountryAddDTO countryAddDto = _fixture.Create<CountryAddDTO>();
            Country country = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            CountryResponseDTO inputCountry=country.ToCountryResponseDTO();
            //act
            _CountryRepoMock.Setup(c => c.GetCountrybyName(It.IsAny<string>())).ReturnsAsync(null as Country);
            _CountryRepoMock.Setup(c => c.AddCountry(It.IsAny<Country>())).ReturnsAsync(country);
           
            CountryResponseDTO responseDto = await _countryAdderService.AddCountry(countryAddDto);
            country.CountryId = responseDto.CountryId;
            inputCountry.CountryId = responseDto.CountryId;
            //assert
            //Assert.Equal("Egypt", responseDto.CountryName);
            responseDto.CountryName.Should().Contain(countryAddDto.CountryName);
            //Assert.Contains(responseDto, _countryService.GetAllCountries());
            inputCountry.Should().BeEquivalentTo(responseDto);
        }
        #endregion

        #region GetAllCountriesTests
        //if the countries list is empty, it should return an empty list
        [Fact]
        public async Task GetAllCountries_CountriesList_IsEmpty()
        {
            //act
            _CountryRepoMock.Setup(c => c.GetAllCountries()).ReturnsAsync(new List<Country>());
            List<CountryResponseDTO> countries =await _countryGetterService.GetAllCountries();
            //Assert
            //Assert.Empty(countries);
            countries.Should().BeEmpty();
        }
        //if the countries list has values,it should return a list of countries
        [Fact]
        public async  Task GetAllCountries_CountriesList_HasValues()
        {
            //arrange
            CountryAddDTO countryAddDTO = _fixture.Create<CountryAddDTO>();
            Country country1 = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            Country country2 = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            List<Country> countries = new List<Country>()
            {
                country1,
                country2
            };
            List<CountryResponseDTO> countryResponseList = countries.Select(c=>c.ToCountryResponseDTO()).ToList();
            _CountryRepoMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countries);
            //act
            List<CountryResponseDTO> countriesFromGetAll =await _countryGetterService.GetAllCountries();
            //Assert
            //Assert.Contains(countryResponseFromAddCountry,countriesFromGetAll);
            countriesFromGetAll.Should().BeEquivalentTo(countryResponseList);
        }
        #endregion

        #region GetCountryById
        //if countryid is Empty,it should return exception
        [Fact]
        public async Task GetCountryById_CountryIdIsNull()
        {
            //arrange
            Guid countryId = Guid.Empty;
            //assert
         Func<Task> getCountryById=() => _countryGetterService.GetCountryByID(countryId);
            //await Assert.ThrowsAsync<ArgumentNullException>(
            //    //act
            //    () => _countryService.GetCountryByID(countryId));
            await getCountryById.Should().ThrowAsync<ArgumentNullException>();
        }
        //if countryid not exist,it should return null
        [Fact]
        public async Task GetCountryById_CountryIdDontExist()
        {
            //arrange
            Guid countryId= Guid.NewGuid();
            //act
            Func<Task> GetCountryByIdResult = async() => await _countryGetterService.GetCountryByID(countryId);
            //assert
            await GetCountryByIdResult.Should().ThrowAsync<ArgumentNullException>();
        }
        //if countryid exists,it should return object of countryresponsedto
        [Fact]
        public async Task GetCountryById_CourseIdExists()
        {
            //arrange
            Country country = _fixture.Build<Country>().With(c=>c.Persons,null as List<Person>).Create();
            //act
            _CountryRepoMock.Setup(c => c.GetCountrybyId(It.IsAny<Guid?>())).ReturnsAsync(country);
            CountryResponseDTO? GetCountryByIdResult =await _countryGetterService.GetCountryByID(country.CountryId);
            //assert
            //Assert.Equal(countryResponseFromAddCountry, GetCountryByIdResult);
            GetCountryByIdResult.Should().Be(country.ToCountryResponseDTO());
        }
        #endregion
    }
}