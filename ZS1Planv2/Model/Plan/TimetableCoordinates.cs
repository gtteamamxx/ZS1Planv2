using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public struct TimetableCoordinates
    {
        public const int DAY_OFFSET = 2;
        public const int LESSON_OFFSET = 1;

        public int DayId { get; set; }
        public int LessonId { get; set; }
    }
}
