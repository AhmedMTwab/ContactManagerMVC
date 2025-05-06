using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? Email { get; set; }
        [RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Phone number must start with '+' followed by 12 digits (e.g., +201234567890)")]
        [StringLength(13, ErrorMessage = "Phone number should be 13 numbers")]
        [MinLength(13, ErrorMessage = "Phone number should be 13 numbers")]
        public string? phoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)] //nvarchar(100)
        public string? Gender { get; set; }

        //uniqueidentifier
        public Guid? CountryID { get; set; }

        public Guid? ApplicationUserID { get; set; }

        [StringLength(200)] //nvarchar(200)
        public string? Address { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }
        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
