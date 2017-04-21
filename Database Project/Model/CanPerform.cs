using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class CanPerform
    {
        [Key]
        [ForeignKey("Specialty")]
        [Column(Order = 0)]
        public int SpecialtyID { get; set; }

        [Key]
        [ForeignKey("MedicalProcedure")]
        [Column(Order = 1)]
        public int MedicalProcedureID { get; set; }

        public Specialty Specialty { get; set; }
        public MedicalProcedure MedicalProcedure { get; set; }
    }
}
