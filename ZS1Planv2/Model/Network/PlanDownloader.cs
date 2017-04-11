using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Network
{
    public class PlanDownloader
    {
        public delegate void DownloadProgressChanged(string name, double percentage);
        public event DownloadProgressChanged OnDownloadProgressChanged;

        private enum Plans
        {
            Classes_Plans = 0,
            Teachers_Plans
        }

        private readonly string TIMETABLE_LIST_URL = $"{TIMETABLE_BASE_URL}lista.html";
        private const string TIMETABLE_BASE_URL = "http://zs-1.pl/plany/";
        private const string NO_LESSON_STRING = "&nbsp;";

        private HtmlParser _Parser;

        public async Task<Plan.Plan> DownloadNewPlanAsync()
        {
            CreateParser();
            Model.Plan.Plan plan = await DownloadPlanAsync();
            return plan;
        }

        private async Task<Plan.Plan> DownloadPlanAsync()
        {
            try
            {
                var timetablesDocument = await _Parser.ParseAsync(await
                    new Network.HtmlDownloader().GetHtmlSource(TIMETABLE_LIST_URL));

                IEnumerable<IElement> plans = timetablesDocument.QuerySelectorAll("ul").Take(2);

                return new Model.Plan.Plan()
                {
                    ClassesPlans = await DownloadClassesPlanAsync(plans.ElementAt((int)Plans.Classes_Plans)),
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CheckLessonName(ref string lessonName, IElement lessonHtml)
        {
            if (!lessonName.Contains("1/2") && !lessonName.Contains("2/2"))
            {
                int positionOfLessonName = lessonHtml.TextContent.IndexOf(lessonName, StringComparison.CurrentCulture);
                string checkString = lessonHtml.TextContent.Substring(positionOfLessonName, lessonHtml.TextContent.Length - positionOfLessonName - 1);
                lessonName += checkString.Contains("1/2") ? "-1/2" : checkString.Contains("2/2") ? "-2/2" : "";
            }
        }

        private bool? GetSecondLessonDetail
            (IElement lessonHtml, IEnumerable<IElement> pList, IEnumerable<IElement> sList,
            IEnumerable<IElement> spanList, Model.Plan.Lesson lesson)
        {
            if (spanList.Count() < 4 || pList.Count() == 1)
                return false;

            string name = string.Empty;
            string detail = string.Empty;
            bool isCheckNameNeeded = false;

            switch (pList.Count())
            {
                case 2:
                    name = pList.ElementAt(1).TextContent;
                    detail = spanList.Where(p => p.ClassName == "n").ElementAt(1).TextContent;
                    break;
                case 3:
                    name = pList.ElementAt(1).TextContent;
                    if (name.Length == 3)
                    {
                        name = pList.ElementAt(2).TextContent;
                        detail = spanList.Where(p => p.ClassName == "n").First().TextContent;
                    }
                    if (detail == "")
                        detail = pList.ElementAt(2).TextContent;
                    isCheckNameNeeded = true;
                    break;
                case 4:
                    name = pList.ElementAt(2).TextContent;
                    detail = pList.ElementAt(3).TextContent;
                    isCheckNameNeeded = true;
                    break;

                default:
                    return null;
            }

            if (isCheckNameNeeded)
                CheckLessonName(ref name, lessonHtml);

            lesson.LessonDetails[1] = detail;
            lesson.LessonNames[1] = name;
            lesson.LessonPlaces[1] = sList.ElementAt(1).TextContent;

            return true;
        }
        private void GetFirstLessonDetail
            (IElement lessonHtml, IEnumerable<IElement> pList, IEnumerable<IElement> sList,
            IEnumerable<IElement> spanList, Model.Plan.Lesson lesson)
        {
            lesson.LessonNames[0] = pList.First().TextContent;
            lesson.LessonPlaces[0] = sList.First().TextContent;

            try
            {
                if (pList.Count() == 3)
                    lesson.LessonDetails[0] = pList.ElementAt(1).TextContent;
                else
                    lesson.LessonDetails[0] = spanList.First(p => p.ClassName == "n").TextContent;
            }
            catch
            {
                lesson.LessonDetails[0] = pList.ElementAt(1).TextContent;
            }

            //sometimes, a class has not a number of class group in p.ClassName.TextContent
            //so we have to check it manually
            if (!lesson.LessonNames[0].Contains("1/2") && !lesson.LessonNames[0].Contains("2/2"))
                lesson.LessonNames[0] += (lessonHtml.TextContent.Contains("1/2") ? "-1/2" : lessonHtml.TextContent.Contains("2/2") ? "-2/2" : "");
        }

        private object ParseLesson(Model.Plan.Lesson lesson, IElement lessonHtml)
        {
            IEnumerable<IElement> spanList = lessonHtml.QuerySelectorAll("span");
            IEnumerable<IElement> pList = spanList.Where(p => p.ClassName == "p");
            IEnumerable<IElement> sList = spanList.Where(p => p.ClassName == "s");

            GetFirstLessonDetail(lessonHtml, pList, sList, spanList, lesson);
            return GetSecondLessonDetail(lessonHtml, pList, sList, spanList, lesson);
        }

        private async Task<object> ParseClassAsync
            (Model.Plan.LessonPlan classLessonPlan, IEnumerable<IElement> hours, int classesCount, int classId)
        {
            int hoursCount = hours.Count();

            double percent = 0.5f + ((classId + 1) * 100.0) / classesCount * 1.0;
            OnDownloadProgressChanged?.Invoke(classLessonPlan.Name, percent);

            for (int d = 0; d < 5; d++)
            {
                Model.Plan.Day day = new Model.Plan.Day();

                for (int hour = 0; hour < hoursCount; hour++)
                {

                    Model.Plan.Lesson lesson = new Model.Plan.Lesson();
                    var lessonHtml = hours.ElementAt(hour).Children[2 + d];

                    if (lessonHtml.InnerHtml == NO_LESSON_STRING)
                    {
                        day.Lessons.Add(lesson);
                        continue;
                    }

                    if (ParseLesson(lesson, lessonHtml) == null)
                        return null;

                    day.Lessons.Add(lesson);
                }

                classLessonPlan.Days.Add(day);

                OnDownloadProgressChanged?.Invoke($"{classLessonPlan.Name} [{d + 1} / {5}]", percent);
                await Task.Delay(10);
            }
            return true;
        }

        private async Task<List<Model.Plan.LessonPlan>> DownloadClassesPlanAsync(IElement classesHtmlElement)
        {
            try
            {
                List<Model.Plan.LessonPlan> classesPlans = new List<Model.Plan.LessonPlan>();
                var classesHtml = classesHtmlElement.QuerySelectorAll("li");
                int classesCount = classesHtml.Count();

                for (int i = 0; i < classesCount; i++)
                {
                    string classUrl = classesHtml[i].FirstElementChild.GetAttribute("href");
                    string url = $"{TIMETABLE_BASE_URL}{classUrl}";

                    var classDocument = await _Parser.ParseAsync(await new Network.HtmlDownloader().GetHtmlSource(url));

                    IEnumerable<IElement> hours = classDocument
                        .QuerySelectorAll("table")
                        .First(p => p.ClassName == "tabela")
                        .QuerySelectorAll("tr")
                        .Where(p => p.FirstElementChild.ClassName == "nr");

                    Model.Plan.LessonPlan classLessonPlan =
                        new Model.Plan.LessonPlan(Plan.LessonPlan.LessonType.Class_Plan)
                        {
                            Name = classDocument.QuerySelector("span").TextContent
                        };

                    if (await ParseClassAsync(classLessonPlan, hours, classesCount, classId: i) == null)
                        return null;
                    classesPlans.Add(classLessonPlan);
                }

                return classesPlans;
            }
            catch
            {
                return null;
            }
        }


        private void CreateParser()
            => _Parser = new HtmlParser();
    }
}
