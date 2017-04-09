using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public class Plan
    {
        public static List<LessonPlan> ClassesPlans;
        public static List<LessonPlan> TeachersPlans;

        public Plan()
        {
            ClassesPlans = new List<LessonPlan>();
            TeachersPlans = new List<LessonPlan>();
        }
    }
}
