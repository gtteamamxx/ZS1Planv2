using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Plan
{
    public class Lesson
    {
        public Lesson(int dayId, int lessonId)
        {
            this.LessonNames = new string[2];
            this.LessonDetails = new string[2];
            this.LessonPlaces = new string[2];

            this.Coordinates = new TimetableCoordinates()
            {
                DayId = dayId,
                LessonId = lessonId
            };
        }

        public string[] LessonNames { get; set; }
        public string[] LessonPlaces { get; set; }
        public string[] LessonDetails { get; set; }

        public TimetableCoordinates Coordinates { get; set; }
    }
}
