using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Project.ViewModel
{
    public class HospitalStatistics
    {
        public float CurrentCosts { get; set; } = 23050.50f;
        public int CurrentDeaths { get; set; } = 13;
        public int CurrentAlive { get; set; } = 3;


        public DateTime CurrentDate = Convert.ToDateTime("01/01/2017");
        public event EventHandler ValuesChanged;


        public void TriggerEvent()
        {
            ValuesChanged(this, new EventArgs());
        }

        /// <summary>
        /// Singleton
        /// </summary>
        private HospitalStatistics() { }
        private static HospitalStatistics instance;
        public static HospitalStatistics Instance {
            get
            {
                return instance ?? (instance = new HospitalStatistics());
            }
        }

    }
}
