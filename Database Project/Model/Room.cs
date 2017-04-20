using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomID { get; set; }
        public int WardID { get; set; }
        public int RoomNumber { get; set; }
        public float Price { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleID { get; set; }
        
        /// <summary>
        /// Operating room or recovery room?
        /// </summary>
        public bool OFlag { get; set; }

        public Schedule Schedule { get; set; }
    }
}
