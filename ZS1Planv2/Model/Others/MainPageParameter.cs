using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Others
{
    public class MainPageParameter
    {
        public MainPageParameter() { }
        public MainPageParameter(bool downloadTimetable = false)
        {
            this.DownloadTimetable = downloadTimetable;
        }

        public bool DownloadTimetable { get; set; }
    }
}
