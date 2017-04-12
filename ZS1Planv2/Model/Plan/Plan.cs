using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public class Plan
    {
        public static Plan Instance;

        public List<LessonPlan> ClassesPlans;

        public Plan()
        {
            ClassesPlans = new List<LessonPlan>();
        }

        public void SetAsMainInstance()
            => Instance = this;
    }
}
