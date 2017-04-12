using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using ZS1Planv2.Model.Application;
using ZS1Planv2.Model.Plan;
using ZS1Planv2.ViewModel;

namespace ZS1Planv2.Model.Others
{
    public static class LessonPlanContentGenerator
    {
        public static object GenerateContentToDisplay(this LessonPlan plan, TimetablePageViewModel viewModel)
        {
            Grid mainGrid = new Grid()
            {
                Background = new SolidColorBrush(Colors.Gray),
            };

            mainGrid.AddRowDefinitions(plan);
            mainGrid.AddColumnDefinitions(plan);
            mainGrid.AddPlanSchemeToGrid(plan);
            mainGrid.Center();

            return mainGrid;
        }

        private static void AddColumnDefinitions(this Grid grid, LessonPlan plan)
        {
            int columnsNum = 7;

            /* 1 - Lesson number"
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

        private static void AddItemToGridByRowAndColumn(this Grid grid, int row, int column, LessonPlan plan)
        {
            TextBlock textBlock = new TextBlock();

            if(row == 0)
            {
                string text = string.Empty;
                switch(column)
                {
                    case 0:
                        text = Text.GetText(Text.TextId.TimetablePage_Grid_Text_1);
                        break;
                    case 1:
                        text = Text.GetText(Text.TextId.TimetablePage_Grid_Text_2);
                        break;
                    default:
                        text = Text.GetText(dayId: column - 2);
                        break;
                }

                textBlock.Text = text;
            }
            else
            {
                if(column == 0)
                    textBlock.Text = $"{row}";
                else
                {
                    //todo
                }
            }

            Grid lessonGrid = new Grid()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Windows.UI.Xaml.Thickness(1),
                Margin = new Thickness(10)
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

        private static void Center(this Grid grid)
        {
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
