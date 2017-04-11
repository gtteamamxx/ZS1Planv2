using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public class LessonPlan
    {
        public enum LessonType
        {
            Class_Plan = 0,
            Teacher_Plan
        }

        public LessonPlan(LessonType type)
        {
            this.Days = new List<Day>();
            this.Type = type;
        }

        public string Name { get; set; }
        public List<Day> Days { get; set; }
        public LessonType Type { get; private set; }

    }
}
