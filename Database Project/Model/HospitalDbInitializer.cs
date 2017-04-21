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
            for (int i = 0; i < 30; i++)
            {
                Schedule roomSchedule = new Schedule() { ScheduleName = Classes.Generator.GetFirstName() };

                context.Rooms.Add(new Room()
                {
                    Price = Classes.Generator.GetPrice(10000),
                    RoomNumber = room++,
                    Schedule = roomSchedule,
                    OFlag = i >= 10,
                    Ward = ward
                });
            }


            base.Seed(context);
        }
    }
}
