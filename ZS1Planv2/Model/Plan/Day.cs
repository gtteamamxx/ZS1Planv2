using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public class Day
    {
        public Day()
        {
            this.Lessons = new List<Lesson>();
        }

        public List<Lesson> Lessons { get; set; }
    }
}
