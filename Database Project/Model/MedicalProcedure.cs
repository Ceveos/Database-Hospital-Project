using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class MedicalProcedure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProcedureID { get; set; }

        public int OperationTime { get; set; }
        public int RecoveryTime { get; set; }
        public float Cost { get; set; }
        public string Name { get; set; }
    }
}
