using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Database_Project.Model
{
    public class MedicalCondition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConditionID { get; set; }
        public string ConditionName { get; set; }

    }
}
