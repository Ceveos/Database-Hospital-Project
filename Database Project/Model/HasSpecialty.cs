using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class HasSpecialty
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonID { get; set;}

        [Key]
        [ForeignKey("Specialty")]
        public int SpecialtyID { get; set; }
        public Person Person { get; set; }
        public Specialty Specialty { get; set; }
    }
}
