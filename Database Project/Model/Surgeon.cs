using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class Surgeon
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public float Wage { get; set; }
        public string Address { get; set; }
        public string PracticeInfor { get; set; }


        public Person Person { get; set; }
    }
}
