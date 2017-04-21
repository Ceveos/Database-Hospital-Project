using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class HasCondition
    {
        [Key]
        [ForeignKey("Person")]
        [Column(Order = 0)]
        public int PersonID { get; set; }
        
        [Key]
        [ForeignKey("MedicalCondition")]
        [Column(Order = 1)]
        public int MedicalConditionID { get; set; }

        public Person Person { get; set; }
        public MedicalCondition MedicalCondition { get; set; }
    }
}
