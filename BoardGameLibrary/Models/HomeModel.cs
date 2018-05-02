using System.Configuration;

namespace BoardGameLibrary.Models
{
    public class HomeModel
    {
        public CopyCheckOutViewModel CheckOut { get; set; }
        public CopyCheckInViewModel CheckIn { get; set; }
        public CopySearchViewModel CopySearch { get; set; }
        public StatisticsModel Statistics { get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationYearBackgroundColor { get; set; }

        public HomeModel()
        {
            CheckIn = new CopyCheckInViewModel();
            CheckOut = new CopyCheckOutViewModel();
            CopySearch = new CopySearchViewModel();
            Statistics = new StatisticsModel();
            ApplicationTitle = ConfigurationManager.AppSettings["ApplicationTitle"];
            ApplicationYearBackgroundColor = ConfigurationManager.AppSettings["InstanceColor"];
        }
    }
}