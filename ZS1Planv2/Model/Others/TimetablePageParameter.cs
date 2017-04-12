using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Others
{
    public class TimetablePageParameter
    {
        public TimetablePageParameter() { }
        public TimetablePageParameter(bool showDefaultPage = true)
        {
            this.ShowDefaultPage = showDefaultPage;
        }
        public TimetablePageParameter(Model.Plan.LessonPlan lessonPlanToShow)
        {
            this.LessonPlanToShow = lessonPlanToShow;
        }

        public bool ShowDefaultPage;
        public Model.Plan.LessonPlan LessonPlanToShow;
    }
}
