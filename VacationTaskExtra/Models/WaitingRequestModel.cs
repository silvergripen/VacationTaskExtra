using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationTaskExtra.Models
{
    public class WaitingRequestModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrentVacId { get; set; }
        public string AcceptReject { get; set; }
    }
}
