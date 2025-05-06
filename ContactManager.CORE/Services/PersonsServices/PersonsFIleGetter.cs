using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.ServiciesContracts.IContryServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using CsvHelper;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContract;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums;

namespace ContactManager.CORE.Servicies.PersonServices
{
    public class PersonsFIleGetter:IPersonsFIleGetter
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonGetter _personGetter;
        private readonly ICountryGetter _countryGetter;
        private readonly IPersonAdder _personAdder;

        //constructor
        public PersonsFIleGetter(IPersonRepository personRepository, IPersonGetter personGetter,ICountryGetter countryGetter,IPersonAdder personAdder)
        {
            _personRepository = personRepository;
            _personGetter = personGetter;
            _countryGetter = countryGetter;
            this._personAdder = personAdder;
        }
        public async Task<MemoryStream> GetAllPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture);
            List<PersonResponseDTO> persons = await _personGetter.GetAllPersons();
            await csvWriter.WriteRecordsAsync(persons);
            await streamWriter.FlushAsync();
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<int> AddPersonsFormExcel(IFormFile personFile)
        {
            MemoryStream stream = new MemoryStream();
            await personFile.CopyToAsync(stream);
            using (ExcelPackage excelpackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];
                int insertedPersons = 0;
                int numberOfRows = worksheet.Dimension.Rows;
                for (int row = 1; row <= numberOfRows; row++)
                {
                    PersonAddDTO rowPerson = new PersonAddDTO();
                    string? cellValue = worksheet.GetValue(row, 1).ToString();
                    if (cellValue != null)
                    {

                        rowPerson.PersonName = worksheet.GetValue<string>(row, 1);
                        rowPerson.phoneNumber = worksheet.GetValue<string>(row, 2);
                        rowPerson.Email = worksheet.GetValue<string>(row, 3);
                        rowPerson.Gender =Enum.Parse<GenderEnum>(worksheet.GetValue<String>(row, 4));
                        rowPerson.DateOfBirth = worksheet.GetValue<DateTime>(row, 5);
                        rowPerson.Address = worksheet.GetValue<string>(row, 6);
                        CountryResponseDTO? rowcountry = await (_countryGetter.GetCountryByName(worksheet.GetValue<string>(row, 7)));
                        
                        rowPerson.CountryID = rowcountry.CountryId;
                        await _personAdder.AddPerson(rowPerson);
                        insertedPersons++;

                    }
                }
                return insertedPersons;

            }
        }

        }
    }
