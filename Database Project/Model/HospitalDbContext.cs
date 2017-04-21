using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Database_Project.Model
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext() : base("Data Source=HospitalDB.sdf;Persist Security Info=False;")
        {
            Database.SetInitializer(new HospitalDbInitializer());
        }
        
        // Type of people
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Surgeon> Surgeons { get; set; }

        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Technician> Technicians {get;set;}

        // List of Conditions
        public DbSet<MedicalCondition> MedicalConditions { get; set; }

        // Patients have x condition
        public DbSet<HasCondition> HasConditions { get; set; }


        // List of specialties
        public DbSet<Specialty> Specialties { get; set; }

        // List of procedures
        public DbSet<MedicalProcedure> Procedures { get; set; }

        // What procedure can cure a condition
        public DbSet<CanCure> CanCure { get; set; }

        // What specialty can perform a procedure
        public DbSet<CanPerform> CanPerform { get; set; }

        // Who contains that specialty
        public DbSet<HasSpecialty> HasSpecialty { get; set; }

        // List of wards in hospital
        public DbSet<Ward> Wards { get; set; }

        // List of rooms in wards
        public DbSet<Room> Rooms { get; set; }

    }
}
