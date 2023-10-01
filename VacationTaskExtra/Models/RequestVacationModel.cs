using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VacationTaskExtra.Models
{
    public class RequestVacationModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestVacId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
        [ForeignKey("VacationType")]
        public int FK_VacationType { get; set; }
        public virtual VacationTypeModel VacationType { get; set; }

        [Required]
        [ForeignKey("Personel")]
        public string FK_Personel { get; set; } = string.Empty;

        public virtual PersonelModel? Personels { get; set; }

        [ForeignKey("WaitingRequest")]

        public int FK_WaitingRequestModel { get; set; }

        public virtual WaitingRequestModel? WaitingRequest { get; set; }

    }
}
