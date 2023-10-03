using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VacationTaskExtra.Models
{
    public class RequestVacationModel
    {
        [NotMapped]
        public int DiffDates
        {
            get
            {
                TimeSpan diff = DateEnd - CurrentDate;
                return (int)diff.TotalDays;
            }
        }

            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestVacId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }

        [DataType(DataType.Date)]
        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public bool isActive { get; set; }
  

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
