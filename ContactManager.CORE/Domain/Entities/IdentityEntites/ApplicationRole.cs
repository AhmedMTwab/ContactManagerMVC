using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.CORE.Domain.Entities.IdentityEntites
{
    public class ApplicationRole:IdentityRole<Guid>
    {
    }
}
