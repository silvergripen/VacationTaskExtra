using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationTaskExtra.Models
{
    public class VacationStatusModel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VacationStatusId { get; set; }
        public bool isActive { get; set; }

        public DateTime CurrentDate { get; set; }

        [Required]
        [ForeignKey("WaitingRequest")]
        public int FK_WaitingRequest { get; set; }

        public virtual WaitingRequestModel WaitingRequest { get; set; }

        [Required]
        [ForeignKey("Personel")]
        public string FK_Personel { get; set; } = string.Empty;

        public virtual PersonelModel? Personel { get; set; }


    }
}
