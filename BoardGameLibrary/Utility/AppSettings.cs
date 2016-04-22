using System.Configuration;

namespace BoardGameLibrary.Utility
{
    public class AppSettings
    {
        private int? _pageSize = null;
        public int PageSize {
            get
            {
                if (!_pageSize.HasValue)
                    _pageSize = int.Parse(ConfigurationManager.AppSettings["IndexPageSize"]);

                return _pageSize.Value;
            }
        }
    }
}