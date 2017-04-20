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
    public class Hospital : DbContext
    {
        protected Hospital(DbCompiledModel model) : base(model)
        {
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

        // What specialty can cure a condition
        public DbSet<CanCure> CanCure { get; set; }

        // Who contains that specialty
        public DbSet<HasSpecialty> HasSpecialty { get; set; }

        // List of wards in hospital
        public DbSet<Ward> Wards { get; set; }

        // List of rooms in wards
        public DbSet<Room> Rooms { get; set; }

    }
}
