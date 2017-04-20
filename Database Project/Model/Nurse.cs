﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class Nurse
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonID { get; set; }

        public string Rank { get; set; }

        public int WardID { get; set; }

        public Person Person { get; set; }
    }
}