using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationTaskExtra.Models
{
    public class TimeLeftModel
    {

        [NotMapped]


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeLeftId { get; set; }
        public int TimeLeft { get; set; }

        [Required]
        [ForeignKey("Personel")]
        public string FK_Personel { get; set; } = string.Empty;

        public virtual PersonelModel? Personel { get; set; }

        [ForeignKey("VacationType")]
        public int FK_VacationType { get; set; }
        public virtual VacationTypeModel VacationType { get; set; }

        [ForeignKey("RequestVacationModel")]

        public int FK_RequestVacationModel { get; set; }

        public virtual RequestVacationModel VacationModel { get; set; }
    }
}
