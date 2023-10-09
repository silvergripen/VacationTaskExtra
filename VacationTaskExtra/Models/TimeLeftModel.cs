using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationTaskExtra.Models
{
    public class TimeLeftModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeLeftId { get; set; }

        public int TimeLeft { get; set; }

        [Required]
        [ForeignKey("Personel")]
        public string FK_Personel { get; set; }

        public virtual PersonelModel Personel { get; set; }

        [ForeignKey("VacationType")]
        public int FK_VacationType { get; set; }
        public virtual VacationTypeModel VacationType { get; set; }
    }
}