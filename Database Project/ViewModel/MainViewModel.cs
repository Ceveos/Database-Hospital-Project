using GalaSoft.MvvmLight;
using System;
using System.Data.SqlClient;

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
        private SqlConnection sqlConnection;
        private float profits = 23050.50f;
        public string Profits
        {
            get
            {
                return $"Costs: {profits:C2}";
            }
        }

        private int deaths = 13;
        public string Deaths {
            get
            {
                return $"Deaths: {deaths}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            //sqlConnection = new SqlConnection("user id=jeffrey;" +
            //                           "password=nebuchadnezzar1;server=inara.asobu.org;" +
            //                           "Trusted_Connection=yes;" +
            //                           "database=Hospital; " +
            //                           "connection timeout=30");
            //try
            //{
            //    sqlConnection.Open();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
        }
    }
}