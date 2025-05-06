using ContactManager.ActionFilters;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.Servicies.PersonServices;
using ContactManager.CORE.ServiciesContracts.IContryServices;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using SerivicesContracts;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums; 
namespace ContactManager.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonGetter _personsGetterService;
        private readonly IPersonsFIleGetter _personsFileGetterService;
        private readonly IPersonAdder _personsAdderService;
        private readonly IPersonSorter _personsSorterService;
        private readonly IPersonDeleter _personsDeleterService;
        private readonly IPersonUpdater _personsUpdaterService;

        private readonly ICountryGetter _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentApplicationUserService _currentApplicationUserService;


        //constructor
        public  PersonsController(IPersonGetter personsGetterService, IPersonAdder personsAdderService, IPersonDeleter personsDeleterService, IPersonUpdater personsUpdaterService, IPersonSorter personsSorterService, ICountryGetter countriesService,IPersonsFIleGetter personsFileGetterService, ILogger<PersonsController> logger,UserManager<ApplicationUser> userManager,ICurrentApplicationUserService currentApplicationUserService)
        {
            _personsGetterService = personsGetterService;
            _personsAdderService = personsAdderService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _personsSorterService = personsSorterService;
            _personsFileGetterService = personsFileGetterService;
            _countriesGetterService = countriesService;
            _logger = logger;
            this._userManager = userManager;
            this._currentApplicationUserService = currentApplicationUserService;
        }
       
        [Route("/")]
        [Route("~/persons/Index")]
        [TypeFilter(typeof(PersonIndexActionFilter))]
        public async Task<IActionResult> Index(string SearchBy,string SearchString,string SortBy=nameof(PersonResponseDTO.PersonName),SortOrderEnum SortOrder = SortOrderEnum.Ascending)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            _currentApplicationUserService.GetCurrentApplicationUserId(currentUser);
            List<PersonResponseDTO> Filterdpersons =await  _personsGetterService.GetFilteredPersons(SearchBy,SearchString);
            List<PersonResponseDTO> sortedPersons = _personsSorterService.GetSortedPersons(Filterdpersons, SortBy, SortOrder);
           
            return View(sortedPersons);
        }
        [HttpGet]
        [Route("~/persons/PersonAsPDF")]
        public async Task<IActionResult> PersonAsPDF()
        {
            return new ViewAsPdf("PersonAsPDF", await _personsGetterService.GetAllPersons(),ViewData);
        }
        [HttpGet]
        [Route("~/persons/PersonAsCSV")]
        public async Task<IActionResult> PersonAsCSV()
        {
            MemoryStream memoryStream = await _personsFileGetterService.GetAllPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }
        [HttpGet]
        [Route("~/persons/Add")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Countries = new SelectList(await _countriesGetterService.GetAllCountries(), nameof(CountryResponseDTO.CountryId), nameof(CountryResponseDTO.CountryName));
            return View();
        }
        [HttpPost]
        [Route("~/persons/Add")]
        public async Task<IActionResult> Add(PersonAddDTO person)
        {
            if (ModelState.IsValid)
            {
                await _personsAdderService.AddPerson(person);
                return RedirectToAction("Index");
            }
            ViewBag.Countries = new SelectList(await _countriesGetterService.GetAllCountries(), nameof(CountryResponseDTO.CountryId), nameof(CountryResponseDTO.CountryName));
            return View(person);
        }

        [HttpGet]
        [Route("~/persons/AddFromExcel")]
        public  IActionResult AddFromExcel()
        {
            return View();
        }
        [HttpPost]
        [Route("~/Persons/AddFromExcel")]
        public async Task<IActionResult> AddFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }
          
            int personsCountInserted = await _personsFileGetterService.AddPersonsFormExcel(excelFile);

            ViewBag.Message = $"{personsCountInserted} Persons Uploaded";
            return View("AddFromExcel");
        }

        [HttpGet]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Edit(Guid PersonID)
        {
           
            PersonResponseDTO EditedPerson =await _personsGetterService.GetPersonById(PersonID);
            if (EditedPerson == null)
            {
                return RedirectToAction("Index");
            }
            
            ViewBag.Countries = new SelectList(await _countriesGetterService.GetAllCountries(), nameof(CountryResponseDTO.CountryId), nameof(CountryResponseDTO.CountryName));
            return View(EditedPerson.ToPersonUpdateRequest());
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Edit(PersonUpdateDTO UpdatedPerson)
        {
            
            PersonResponseDTO EditedPerson =await _personsGetterService.GetPersonById(UpdatedPerson.PersonID);
            if (EditedPerson == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                await _personsUpdaterService.UpdatePerson(UpdatedPerson);
                return RedirectToAction("Index");
            }

            ViewBag.Countries = new SelectList(await _countriesGetterService.GetAllCountries(), nameof(CountryResponseDTO.CountryId), nameof(CountryResponseDTO.CountryName));
            return View(EditedPerson.ToPersonUpdateRequest());
        }
        [HttpGet]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete(Guid PersonID)
        {
          
            PersonResponseDTO DeletePerson =await _personsGetterService.GetPersonById(PersonID);
            if (DeletePerson == null)
            {
                return RedirectToAction("Index");
            }

            
            return View(DeletePerson.ToPersonUpdateRequest());
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete(PersonUpdateDTO UpdatedPerson)
        {
            
            PersonResponseDTO EditedPerson =await _personsGetterService.GetPersonById(UpdatedPerson.PersonID);
            if (EditedPerson == null)
            {
                return RedirectToAction("Index");
            }
            
            await _personsDeleterService.DeletePerson(UpdatedPerson.PersonID);
            
            return RedirectToAction("Index");
        }
    }
}
