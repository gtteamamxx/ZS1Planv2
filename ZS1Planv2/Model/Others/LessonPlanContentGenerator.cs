using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using ZS1Planv2.Model.Application;
using ZS1Planv2.Model.Plan;
using ZS1Planv2.ViewModel;

namespace ZS1Planv2.Model.Others
{
    public static class LessonPlanContentGenerator
    {
        private static ApplicationTheme _CurrentTheme => App.Current.RequestedTheme;
        private static readonly Brush _INFO_BRUSH = new SolidColorBrush(
            _CurrentTheme == ApplicationTheme.Light ? Colors.LightCyan : Color.FromArgb(127, 0, 150, 0));
        private static readonly Brush _BCKG_BRUSH = new SolidColorBrush(Colors.Transparent);
        private static readonly Brush _BORDER_BRUSH = new SolidColorBrush(Colors.Brown);
        private static readonly Brush _PLACE_FONT_BRUSH = new SolidColorBrush(
            _CurrentTheme == ApplicationTheme.Light ? Colors.Red : Colors.Cyan);

        public static object GenerateContentToDisplay(this LessonPlan plan, TimetablePageViewModel viewModel)
        {
            Grid mainGrid = new Grid();

            mainGrid.AddRowDefinitions(plan);
            mainGrid.AddColumnDefinitions(plan);
            mainGrid.AddPlanSchemeToGrid(plan);

            return mainGrid;
        }

        private static void AddColumnDefinitions(this Grid grid, LessonPlan plan)
        {
            int columnsNum = 7;

            /* 1 - Lesson number
             * 2 - Hours of lesson
             * 3 - Monday
             * 4 - Tuesdey
             * 5 ...
            */

            Enumerable.Range(1, columnsNum).ToList().ForEach(
                i => grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = GridLength.Auto }));
        }

        private static void AddRowDefinitions(this Grid grid, LessonPlan plan)
        {
            int rowsNum = plan.Days.Max(p => p.Lessons.Count());

            // +1 because first row is title
            Enumerable.Range(1, rowsNum + 1).ToList().ForEach(
                i => grid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto }));
        }

        private static void AddItemToGridByRowAndColumn(this Grid grid, int row, int column,
            LessonPlan plan)
        {
            TextBlock textBlock = new TextBlock()
            {
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            Brush backgroundForGrid = _BCKG_BRUSH;

            SetTextToTextBlockByLessonPlan(textBlock, ref backgroundForGrid, row, column, plan);

            Grid lessonGrid = new Grid()
            {
                BorderBrush = _BORDER_BRUSH,
                BorderThickness = new Windows.UI.Xaml.Thickness(1),
                Background = backgroundForGrid
            };

            lessonGrid.Children.Add(textBlock);

            Grid.SetRow(lessonGrid, row);
            Grid.SetColumn(lessonGrid, column);

            grid.Children.Add(lessonGrid);
        }

        private static void AddPlanSchemeToGrid(this Grid grid, LessonPlan plan)
        {
            int columnsNum = grid.ColumnDefinitions.Count();
            int rowsNum = grid.RowDefinitions.Count();

            for (int column = 0; column < columnsNum; column++)
                for (int row = 0; row < rowsNum; row++)
                    grid.AddItemToGridByRowAndColumn(row, column, plan);
        }

        private static void SetTextToTextBlockByLessonPlan(TextBlock textBlock, ref Brush gridBackground, int row, int column,
            Model.Plan.LessonPlan plan)
        {
            if (row == 0)
                textBlock.SetRowZeroText(ref gridBackground, column);
            else
            {
                if (column == 0 || column == 1)
                {
                    gridBackground = _INFO_BRUSH;
                    textBlock.TextAlignment = TextAlignment.Center;
                }

                textBlock.SetDetailText(ref gridBackground, row, column, plan);
            }
        }

        private static void SetDetailText(this TextBlock textBlock, ref Brush backgroundForGrid, int row, int column,
            Model.Plan.LessonPlan plan)
        {
            if (column == 0)
                textBlock.Text = $"{row}";
            else if (column == 1)
                textBlock.Text = Text.GetText(Text.DataType.Hour, row - TimetableCoordinates.LESSON_OFFSET);
            else
            {
                Model.Plan.Lesson lesson = plan.Days.ElementAt(column - TimetableCoordinates.DAY_OFFSET)
                    .Lessons.First(p => p.Coordinates.LessonId == row - TimetableCoordinates.LESSON_OFFSET);

                Inline firstInline = new Run() { Text = $"{lesson.LessonNames[0]}" };
                Inline secondInline = new Run() { Text = $" {lesson.LessonDetails[0]}", FontWeight = FontWeights.Bold };
                Inline thirdInline = new Run() { Text = $" {lesson.LessonPlaces[0]}", Foreground = _PLACE_FONT_BRUSH };
                Inline fourthInline = new Run() { Text = $"{Environment.NewLine}{lesson.LessonNames[1]}" };
                Inline fifthInline = new Run() { Text = $" {lesson.LessonDetails[1]}", FontWeight = FontWeights.Bold };
                Inline sixthInline = new Run() { Text = $" {lesson.LessonPlaces[1]}", Foreground = _PLACE_FONT_BRUSH };

                textBlock.Inlines.Add<Inline>(firstInline).Add<Inline>(secondInline).Add<Inline>(thirdInline)
                    .Add<Inline>(fourthInline).Add<Inline>(sixthInline);
            }
        }

        private static void SetRowZeroText(this TextBlock textBlock,ref Brush backgroundForGrid, int column)
        {
            string text = string.Empty;
            backgroundForGrid = _INFO_BRUSH;

            switch (column)
            {
                case 0:
                    text = Text.GetText(Text.TextId.TimetablePage_Grid_Text_1);
                    break;
                case 1:
                    text = Text.GetText(Text.TextId.TimetablePage_Grid_Text_2);
                    break;
                default:
                    text = Text.GetText(Text.DataType.DayName, column - TimetableCoordinates.DAY_OFFSET);
                    break;
            }

            textBlock.Text = text;
            textBlock.TextAlignment = TextAlignment.Center;
        }

        private static ICollection<Inline> Add<Inline>(this ICollection<Inline> collection, Inline add)
        {
            collection.Add(add);
            return collection;
        }
    }
}
