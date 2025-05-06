using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContactManager.CORE.ServiciesContracts.IPersonServices
{
    public interface IPersonsFIleGetter
    {
        public Task<int> AddPersonsFormExcel(IFormFile personAdd);

        public Task<MemoryStream> GetAllPersonsCSV();

    }
}
