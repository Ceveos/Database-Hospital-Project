using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class CanCure
    {
        [Key]
        [ForeignKey("MedicalProcedure")]
        public int MedicalProcedureID { get; set; }

        [Key]
        [ForeignKey("MedicalCondition")]
        public int MedicalConditionID { get; set; }

        public float Probability { get; set; }
        public MedicalProcedure MedicalProcedure { get; set; }
        public MedicalCondition MedicalCondition { get; set; }
    }
}
