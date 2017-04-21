using GalaSoft.MvvmLight;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Project.ViewModel
{
    public class GraphViewModel : ViewModelBase
    {
        private HospitalStatistics hospitalStatistics = HospitalStatistics.Instance;
        public HospitalStatistics HospitalStatistics {
            get
            {
                return hospitalStatistics;
            }
        }


        private ChartValues<int> curAlive;
        private ChartValues<int> curDead;
        public ChartValues<int> Alive
        {
            get
            {
                return curAlive;
            }
        }

        public ChartValues<int> Dead
        {
            get
            {
                return curDead;
            }
        }


        public GraphViewModel()
        {

            hospitalStatistics.ValuesChanged += UpdateValues;
            curAlive = new ChartValues<int> { hospitalStatistics.CurrentAlive };
            curDead = new ChartValues<int> { hospitalStatistics.CurrentDeaths };
        }

        private void UpdateValues(object sender, EventArgs e)
        {

            RaisePropertyChanged(nameof(this.HospitalStatistics.CurrentAlive));
            RaisePropertyChanged(nameof(this.HospitalStatistics.CurrentDeaths));

            if (curAlive[0] != hospitalStatistics.CurrentAlive)
            {
                curAlive = new ChartValues<int> { hospitalStatistics.CurrentAlive };
                RaisePropertyChanged(nameof(Alive));
                RaisePropertyChanged(nameof(curAlive));
            }

            if (curDead[0] != hospitalStatistics.CurrentDeaths)
            {
                curDead = new ChartValues<int> { hospitalStatistics.CurrentDeaths };
                RaisePropertyChanged(nameof(Dead));
                RaisePropertyChanged(nameof(curDead));
            }
        }
    }
}
