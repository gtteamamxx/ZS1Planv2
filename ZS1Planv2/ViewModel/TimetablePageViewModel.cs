using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using ZS1Planv2.Model.Application;
using ZS1Planv2.Model.Others;

namespace ZS1Planv2.ViewModel
{
    public class TimetablePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands

        private RelayCommand _PageLoadedCommand;
        public RelayCommand PageLoadedCommand =>
            _PageLoadedCommand ?? (_PageLoadedCommand = new RelayCommand(() => PageLoaded()));

        private RelayCommand _MainMenuButtonCommand;
        public RelayCommand MainMenuButtonCommand =>
            _MainMenuButtonCommand ?? (_MainMenuButtonCommand = new RelayCommand(() => MainMenuButton_Click()));

        private RelayCommand<object> _PageSizeChangedCommand;
        public RelayCommand<object> PageSizeChangedCommand =>
            _PageSizeChangedCommand ?? (_PageSizeChangedCommand = new RelayCommand<object>(o => PageSizeChanged(o)));

        #endregion

        #region Properties

        private object _ScrollViewerContent;
        public object ScrollViewerContent
        {
            get => _ScrollViewerContent;
            set
            {
                _ScrollViewerContent = value;
                OnPropertyChanged();
            }
        }

        private string _MainMenuButtonText;
        public string MainMenuButtonText
        {
            get => _MainMenuButtonText;
            set
            {
                _MainMenuButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _SplitViewIsPaneOpen;
        public bool SplitViewIsPaneOpen
        {
            get => _SplitViewIsPaneOpen;
            set
            {
                _SplitViewIsPaneOpen = value;
                OnPropertyChanged();
            }
        }

        private string _TitleText;
        public string TitleText
        {
            get => _TitleText;
            set
            {
                _TitleText = value;
                OnPropertyChanged();
            }
        }

        private List<Model.Plan.LessonPlan> _LessonsPlans;
        public List<Model.Plan.LessonPlan> LessonsPlans
        {
            get => _LessonsPlans;
            set
            {
                if (_LessonsPlans == value)
                    return;
                _LessonsPlans = value;
                OnPropertyChanged();
            }
        }

        private double _ContentHeight;
        public double ContentHeight
        {
            get => _ContentHeight;
            set
            {
                if (_ContentHeight == value)
                    return;
                _ContentHeight = value;
                OnPropertyChanged();
            }
        }

        private Model.Plan.LessonPlan _SelectedPlan;
        public Model.Plan.LessonPlan SelectedPlan
        {
            get => _SelectedPlan;
            set
            {
                _SelectedPlan = value;
                ShowLessonPlan(value);
                OnPropertyChanged();
            }
        }
        #endregion

        private bool _ShowDefaultPage;
        private Model.Plan.LessonPlan _LessonPlanToShow;
        private Model.Plan.LessonPlan _ActuallyShowingPlan;

        private void ShowLessonPlan(Model.Plan.LessonPlan plan)
        {
            if (plan == null)
                plan = Model.Plan.Plan.Instance.ClassesPlans.First();

            if(_ActuallyShowingPlan == plan)
                return;

            if(_ActuallyShowingPlan == null)
            {
                _ActuallyShowingPlan = plan;
                TitleText = plan.Name;
                ScrollViewerContent = plan.GenerateContentToDisplay(this);
                return;
            }

            FrameHelper.NavigateToPage(typeof(View.TimetablePage),
                new TimetablePageParameter()
                {
                    ShowDefaultPage = plan == null,
                    LessonPlanToShow = plan
                });
        }

        private void PageLoaded()
        {
            LoadLessonPlansToMenu();

            if (this._ShowDefaultPage)
            {
                this._ShowDefaultPage = false;
                ShowDefaultPage();
                return;
            }

            if (_LessonPlanToShow != null)
            {
                SelectedPlan = _LessonPlanToShow;
                this._LessonPlanToShow = null;
                return;
            }

            ShowDefaultPage();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SetPageTexts();
            if (e.Parameter == null
                || !(e.Parameter is TimetablePageParameter))
                return;

            TimetablePageParameter pageParameter = e.Parameter as TimetablePageParameter;

            if (pageParameter.ShowDefaultPage)
            {
                this._ShowDefaultPage = true;
                return;
            }

            this._LessonPlanToShow = pageParameter.LessonPlanToShow;
        }

        private void PageSizeChanged(object state)
        {
            double newHeight = (state as SizeChangedEventArgs).NewSize.Height;
            ContentHeight = newHeight - 50.0; //50.0 is RelativePanel at Top Height
        }

        private void SetPageTexts()
        {
            MainMenuButtonText = Text.GetText(Text.TextId.TimetablePage_MainMenuButton_Text_1);
            TitleText = Text.GetText(Text.TextId.MainPage_Loading_Text_1);
        }

        private void ShowDefaultPage()
            => SelectedPlan = null;

        private void MainMenuButton_Click()
            => SplitViewIsPaneOpen = !SplitViewIsPaneOpen;

        private void LoadLessonPlansToMenu()
            => LessonsPlans = Model.Plan.Plan.Instance.ClassesPlans;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
