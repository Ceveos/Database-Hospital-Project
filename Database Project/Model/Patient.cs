using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Model
{
    public class Patient
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonID {get;set;}
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [ForeignKey("NextOfKin")]
        public int NextOfKinID { get; set; }

        [ForeignKey("LivingWill")]
        public int LivingWillID { get; set; }

        [ForeignKey("InsuranceCompany")]
        public int InsuranceCompanyID { get; set; }

        public Person Person { get; set; }
        public Person NextOfKin { get; set; }
        public LivingWill LivingWill { get; set; }
        public InsuranceCompany InsuranceCompany { get; set; }
    }
}
