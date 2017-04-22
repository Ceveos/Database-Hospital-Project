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


        public String CurrentDate
        {
            get
            {
                return hospitalStatistics.CurrentDate.ToString("MM/dd/yyyy");
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
            int patientsToAdd = _rand.Next(0, 5);
            if (Classes.Generator.GetProbability() < 0.30)
                patientsToAdd = 0;

            // Add patients
            for ( int i = 0; i < patientsToAdd; i++)
            {
                // Define a new patient
                Model.Patient patient = new Model.Patient()
                {
                    Name = Classes.Generator.GetFirstName()
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

            hospitalStatistics.TriggerEvent();
        }

        private void UpdateValues(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(this.CurrentDate));
            RaisePropertyChanged(nameof(this.Costs));
            RaisePropertyChanged(nameof(this.Deaths));
            RaisePropertyChanged(nameof(this.LatestPatients));
        }
    }
}