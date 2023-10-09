using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VacationTaskExtra.Models
{
    public partial class PersonelModel : IdentityUser
    {
        

        [MaxLength(50)]
        [Required]
        [DisplayName("Hela namnet")]
        public string FullName { get; set; } = null!;
        [Required]
        public bool IsAdmin { get; set; }

        [ForeignKey("RequestVacation")]
        public int FK_RequestVacation { get; set; }
        public virtual ICollection<RequestVacationModel>? RequestVacations { get; set; }

    }
}
