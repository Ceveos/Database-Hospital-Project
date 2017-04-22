using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Project.Model
{
    class HospitalDbInitializer : DropCreateDatabaseAlways<HospitalDbContext>
    {
        protected override void Seed(HospitalDbContext context)
        {
            Random rand = new Random();


            // Generate a specialty
            Specialty masterSpecialty = new Specialty() { SpecialtyName = "Master" };

            context.Specialties.Add(masterSpecialty);

            // Generate a procedure
            MedicalProcedure masterProcedure = new MedicalProcedure() {
                Cost = Classes.Generator.GetPrice(10000),
                Name = "Master Procedure",
                OperationTime = 1,
                RecoveryTime = 2
            };

            context.Procedures.Add(masterProcedure);

            // Allow specialty to perform procedure
            context.CanPerform.Add(new CanPerform()
            {
                MedicalProcedure = masterProcedure,
                Specialty = masterSpecialty
            });

            // Seed the database with a list of medical conditions
            foreach (string condition in Dictionaries.Conditions.ListOfConditions)
            {
                MedicalCondition medCondition = new MedicalCondition() { ConditionName = condition };
                context.MedicalConditions.Add(medCondition);

                context.CanCure.Add(new CanCure()
                {
                    MedicalCondition = medCondition,
                    MedicalProcedure = masterProcedure,
                    Probability = Classes.Generator.GetProbability(50, 100)
                });
            }

            // Add a ward
            Ward ward = new Ward() { Name = "Default Ward" };
            context.Wards.Add(ward);

            // Generate rooms. Twice as many recovery rooms as procedures
            int room = 100;
            for (int i = 0; i < 25; i++)
            {
                context.Rooms.Add(new Room()
                {
                    Price = Classes.Generator.GetPrice(10000),
                    RoomNumber = room++,
                    Schedule = new Schedule(),
                    OFlag = i < 10,
                    Ward = ward
                });
            }

            // Add surgeons/nurses/technicians
            for (int i = 0; i < 10; i++)
            {

                Surgeon newSurgeon = new Surgeon()
                {
                    Name = Classes.Generator.GetFirstName(),
                    Schedule = new Schedule(), 
                    Wage = Classes.Generator.GetPrice(10000)
                };

                Nurse newNurse = new Nurse()
                {
                    Name = Classes.Generator.GetFirstName(),
                    Schedule = new Schedule(),
                    WardID = ward.WardID
                };

                Technician newTech = new Technician()
                {
                    Name = Classes.Generator.GetFirstName(),
                    Schedule = new Schedule(),
                    Wage = Classes.Generator.GetPrice(10000)
                };

                context.Surgeons.Add(newSurgeon);
                context.Nurses.Add(newNurse);
                context.Technicians.Add(newTech);
            }


            base.Seed(context);
        }
    }
}
