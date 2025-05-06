using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.CORE.ServiciesContracts.IPersonServices
{
    public interface IPersonDeleter
    {
        public Task<bool> DeletePerson(Guid? personId);

    }
}
