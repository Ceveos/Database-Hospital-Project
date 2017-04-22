using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Database_Project.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Model.HospitalDbContext dbContext;
        private static Random _rand = new Random();
        private HospitalStatistics hospitalStatistics = HospitalStatistics.Instance;

        public string Costs
        {
            get
            {
                return $"Costs: {hospitalStatistics.CurrentCosts:C2}";
            }
        }

        public string Deaths {
            get
            {
                return $"Deaths: {hospitalStatistics.CurrentDeaths}";
            }
        }

        public string LatestDeathReason { get; set; }


        public String CurrentDate
        {
            get
            {
                return hospitalStatistics.CurrentDate.ToString("MM/dd/yyyy");
            }
        }

        private int _availableSurgeons;
        public String AvailableSurgeons
        {
            get
            {
                return "Surgeons: " + _availableSurgeons;
            }
        }

        private int _availableNurses;
        public String AvailableNurses
        {
            get
            {
                return "Nurses: " + _availableNurses;
            }
        }


        private int _availableTechnicians;
        public String AvailableTechnicians
        {
            get
            {
                return "Technicians: " + _availableTechnicians;
            }
        }

        private int _availableOperatingRooms;
        public String AvailableOperatingRoom
        {
            get
            {
                return "Operating Room: " + _availableOperatingRooms;
            }
        }


        private int _availableRecoveryRooms;
        public String AvailableRecoveryRoom
        {
            get
            {
                return "Recovery Room: " + _availableRecoveryRooms;
            }
        }

        private int _patientsNeedingSurgery;
        public String PatientsNeedingSurgery
        {
            get
            {
                return "Waiting for surgery: " + _patientsNeedingSurgery;
            }
        }

        private int _patientsNeedingRecovery;
        public String PatientsNeedingRecovery
        {
            get
            {
                return "Waiting for recovery: " + _patientsNeedingRecovery;
            }
        }

        public ObservableCollection<Model.Patient> LatestPatients { get; set; } = new ObservableCollection<Model.Patient>();

        public RelayCommand ProgressDay { get; set; }
        public RelayCommand Stats { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            dbContext = new Model.HospitalDbContext();

            ProgressDay = new RelayCommand(ProgressTime, CanProgressTime);

            Stats = new RelayCommand(ShowStats);

            hospitalStatistics.ValuesChanged += UpdateValues;
        }

        public void ShowStats()
        {
            View.DeathsGraph graph = new View.DeathsGraph();
            graph.Show();
        }

        public bool CanProgressTime()
        {
            return true;
        }


        public void ProgressTime()
        {
            
            // Add a day 
            hospitalStatistics.CurrentDate = hospitalStatistics.CurrentDate.AddDays(1);

            // Determine chance of adding patient(s)
            int patientsToAdd = _rand.Next(1, 15);
            if (Classes.Generator.GetProbability() < 0.20)
                patientsToAdd = 0;

            // Add patients
            for ( int i = 0; i < patientsToAdd; i++)
            {
                // Define a new patient
                Model.Patient patient = new Model.Patient()
                {
                    Name = Classes.Generator.GetFirstName(),
                    Schedule = new Model.Schedule()
                };

                
                // Add him to the database
                dbContext.Patients.Add(patient);

                // Add him to list of recent patients:
                LatestPatients.Add(patient);

                // Remove old patient from the recent list
                if (LatestPatients.Count > 11)
                {
                    LatestPatients.RemoveAt(0);
                }


                // Give him a disease
                dbContext.HasConditions.Add(new Model.HasCondition()
                {
                    Person = patient,
                    MedicalCondition = dbContext.MedicalConditions.OrderBy(r => Guid.NewGuid()).First()
                });

                hospitalStatistics.CurrentAlive++;
            }

            dbContext.SaveChanges();

            DateTime yesterday = hospitalStatistics.CurrentDate.Date.AddDays(-1);

            // Get available surgeons
            var availableSurgeons = dbContext.Surgeons.Where(x => dbContext.Timeblocks.Where(y => y.ScheduleID == x.ScheduleID).All(y => y.Date != hospitalStatistics.CurrentDate.Date)).ToList();

            // Get available nurses
            var availableNurses = dbContext.Nurses.Where(x => dbContext.Timeblocks.Where(y => y.ScheduleID == x.ScheduleID).All(y => y.Date != hospitalStatistics.CurrentDate.Date)).ToList();

            // Get available operating rooms
            var availableOperatingRooms = dbContext.Rooms.Where(x => x.OFlag && dbContext.Timeblocks.Where(y => y.ScheduleID == x.ScheduleID).All(y => y.Date != hospitalStatistics.CurrentDate.Date)).ToList();
            
            // Get available recovery rooms
            var availableRecoveryRooms = dbContext.Rooms.Where(x => !x.OFlag && dbContext.Timeblocks.Where(y => y.ScheduleID == x.ScheduleID).All(y => y.Date != hospitalStatistics.CurrentDate.Date)).ToList();
            
            // Get available technicians
            var availableTechnicians = dbContext.Technicians.Where(x => dbContext.Timeblocks.Where(y => y.ScheduleID == x.ScheduleID).All(y => y.Date != hospitalStatistics.CurrentDate.Date)).ToList();

            // Get a list of patients that aren't treated and doesn't have a time block
            var patientsNeedingSurgery = dbContext.Patients.Where(x => !x.Dead && !dbContext.Timeblocks.Any(y => y.ScheduleID == x.ScheduleID)).ToList();

            // Get a list of patients that aren't treated and had an operation yesterday
            var patientsNeedingRecovery = dbContext.Patients.Where(x => !x.Treated && dbContext.Timeblocks.Any(y => y.ScheduleID == x.ScheduleID
                && y.Date == yesterday && y.OFlag)).ToList();



            // See if we can put a patient into surgery 
            while (patientsNeedingSurgery.Any() &&
                availableSurgeons.Any() && availableTechnicians.Any() && availableOperatingRooms.Any())
            {
                hospitalStatistics.CurrentCosts += Classes.Generator.GetPrice(15000);

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = true,
                    Schedule = patientsNeedingSurgery.First().Schedule,
                    Operation = new Model.Operation()
                });

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = true,
                    Schedule = availableSurgeons.First().Schedule,
                    Operation = new Model.Operation()
                });

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = true,
                    Schedule = availableTechnicians.First().Schedule,
                    Operation = new Model.Operation()
                });

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = true,
                    Schedule = availableOperatingRooms.First().Schedule,
                    Operation = new Model.Operation()
                });

                patientsNeedingSurgery.RemoveAt(0);
                availableSurgeons.RemoveAt(0);
                availableTechnicians.RemoveAt(0);
                availableOperatingRooms.RemoveAt(0);
            }

            dbContext.SaveChanges();

            // See if we can put a surgery patient into recovery
            while (patientsNeedingRecovery.Any() &&
                availableNurses.Any() && availableRecoveryRooms.Any())
            {
                hospitalStatistics.CurrentCosts += Classes.Generator.GetPrice(7000);

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = false,
                    Schedule = patientsNeedingRecovery.First().Schedule,
                    Operation = new Model.Operation()
                });

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = false,
                    Schedule = availableNurses.First().Schedule,
                    Operation = new Model.Operation()
                });

                dbContext.Timeblocks.Add(new Model.TimeBlock()
                {
                    Date = hospitalStatistics.CurrentDate,
                    OFlag = false,
                    Schedule = availableRecoveryRooms.First().Schedule,
                    Operation = new Model.Operation()
                });

                patientsNeedingRecovery.RemoveAt(0);
                availableNurses.RemoveAt(0);
                availableRecoveryRooms.RemoveAt(0);
            }

            dbContext.SaveChanges();



            this._availableSurgeons = availableSurgeons.Count();
            this._availableTechnicians = availableTechnicians.Count();
            this._availableNurses = availableNurses.Count();
            this._availableOperatingRooms = availableOperatingRooms.Count();
            this._availableRecoveryRooms = availableRecoveryRooms.Count();
            this._patientsNeedingSurgery = patientsNeedingSurgery.Count();
            this._patientsNeedingRecovery = patientsNeedingRecovery.Count();


            // Couldn't get surgery? DEAD
            foreach (var deadPatient in patientsNeedingSurgery)
            {
                //dbContext.Patients.First(x => x == deadPatient);
                deadPatient.Dead = true;
                hospitalStatistics.CurrentDeaths++;
                hospitalStatistics.CurrentAlive--;
                this.LatestDeathReason = $"Died from {dbContext.HasConditions.First(x => x.PersonID == deadPatient.PersonID).MedicalCondition.ConditionName}";
            }

            dbContext.SaveChanges();

            // Couldn't get into a recovery room? DEAD
            foreach (var deadPatient in patientsNeedingRecovery)
            {
                //dbContext.Patients.First(x => x == deadPatient);
                deadPatient.Dead = true;
                hospitalStatistics.CurrentDeaths++;
                hospitalStatistics.CurrentAlive--;
                this.LatestDeathReason = $"Died from lack of recovery room";
            }

            dbContext.SaveChanges();

            hospitalStatistics.TriggerEvent();
        }

        private void UpdateValues(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(this.CurrentDate));
            RaisePropertyChanged(nameof(this.Costs));
            RaisePropertyChanged(nameof(this.Deaths));
            RaisePropertyChanged(nameof(this.AvailableNurses));
            RaisePropertyChanged(nameof(this.AvailableSurgeons));
            RaisePropertyChanged(nameof(this.AvailableTechnicians));
            RaisePropertyChanged(nameof(this.AvailableOperatingRoom));
            RaisePropertyChanged(nameof(this.AvailableRecoveryRoom));
            RaisePropertyChanged(nameof(this.PatientsNeedingSurgery));
            RaisePropertyChanged(nameof(this.PatientsNeedingRecovery));
            RaisePropertyChanged(nameof(this.LatestDeathReason));
        }
    }
}