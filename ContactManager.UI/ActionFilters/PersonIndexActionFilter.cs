using ContactManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SerivicesContracts.DTO;

namespace ContactManager.ActionFilters
{
    public class PersonIndexActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        public readonly ILogger<PersonIndexActionFilter> _logger;
        public PersonIndexActionFilter(ILogger<PersonIndexActionFilter> logger)
        {
            _logger = logger;
        }

        public int Order {get;set;}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}: {Method}",nameof(PersonIndexActionFilter),"onActionExecuting");
            PersonsController personsController = (PersonsController)context.Controller;
            Dictionary<string, string> SearchByList = new Dictionary<string, string>()
            {
                { nameof(PersonResponseDTO.PersonName),"Person Name"},
                { nameof(PersonResponseDTO.Email),"Person Email"},
                { nameof(PersonResponseDTO.DateOfBirth),"DateOfBirth"},
                { nameof(PersonResponseDTO.Age),"Age"},
                { nameof(PersonResponseDTO.Gender),"Gender"},
                { nameof(PersonResponseDTO.Country),"Country"}
            };
            personsController.ViewBag.SearchBy = SearchByList;
            IDictionary<string, object?>? actionArguments = context.ActionArguments;
           
            await next();
            if(actionArguments.ContainsKey("SearchBy"))
            { 
            personsController.ViewBag.currentSearchBy = Convert.ToString(actionArguments["SearchBy"]);
            }
            if (actionArguments.ContainsKey("SearchString"))
            {
                personsController.ViewBag.currentSearchString = Convert.ToString(actionArguments["SearchString"]);
            }
            if (actionArguments.ContainsKey("SortBy"))
            {
                personsController.ViewBag.currentSortBy =Convert.ToString( actionArguments["SortBy"]);
            }
            if (actionArguments.ContainsKey("SortOrder"))
            {
                personsController.ViewBag.currentSortOrder = Convert.ToString(actionArguments["SortOrder"]);
            }

        }
    }
}
