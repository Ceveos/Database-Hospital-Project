using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class TimeBlock
    {
        [Key]
        [ForeignKey("Schedule")]
        public int ScheduleID { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Operation")]
        public int OperationID { get; set; }

        public bool OFlag { get; set; }

        public Schedule Schedule { get; set; }
        public Operation Operation { get; set; }
    }
}
