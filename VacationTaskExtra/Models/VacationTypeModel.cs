using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationTaskExtra.Models
{
    public class VacationTypeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [MaxLength(50)]
        public string TypeName { get; set; }
        public int maxTime { get; set; }
    }
}
