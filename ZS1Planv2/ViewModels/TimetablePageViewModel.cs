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

        public TimetablePageViewModel()
        {
            MVVMMessagerService.RegisterReceiver<bool>(typeof(TimetablePageViewModel), refreshTimetable =>
            {
                if(refreshTimetable)
                {
                    Model.Plan.LessonPlan tempPlan = _ActuallyShowingPlan;
                    _ActuallyShowingPlan = null;
                    ShowLessonPlan(tempPlan);
                }
            });
        }

        #region Commands

        private RelayCommand _PageLoadedCommand;
        public RelayCommand PageLoadedCommand =>
            _PageLoadedCommand ?? (_PageLoadedCommand = new RelayCommand(() => PageLoaded()));

        private RelayCommand _MainMenuButtonCommand;
        public RelayCommand MainMenuButtonCommand =>
            _MainMenuButtonCommand ?? (_MainMenuButtonCommand = new RelayCommand(() => MainMenuButton_Click()));

        private RelayCommand _RefreshTimetableButtonCommand;
        public RelayCommand RefreshTimetableButtonCommand =>
            _RefreshTimetableButtonCommand ?? (_RefreshTimetableButtonCommand = new RelayCommand(() => RefreshTimetableButton_Click()));

        private RelayCommand<object> _PageUnloadedCommand;
        public RelayCommand<object> PageUnloadedCommand =>
            _PageUnloadedCommand ?? (_PageUnloadedCommand = new RelayCommand<object>((ob) => PageUnloaded(ob)));

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

        private string _SettingsButtonText;
        public string SettingsButtonText
        {
            get => _SettingsButtonText;
            set
            {
                _SettingsButtonText = value;
                OnPropertyChanged();
            }
        }

        private string _RefreshTimetableButtonText;
        public string RefreshTimetableButtonText
        {
            get => _RefreshTimetableButtonText;
            set
            {
                _RefreshTimetableButtonText = value;
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
            {
                Model.Plan.LessonPlan planToShow = Model.Plan.Plan.Instance.ClassesPlans.FirstOrDefault(
                    p => p.Name == Model.Application.SettingsManager.GetSetting<string>(SettingsManager.SettingsType.Start_Plan));

                plan = planToShow ?? Model.Plan.Plan.Instance.ClassesPlans.First();
            }

            if(_ActuallyShowingPlan == plan)
                return;

            if(_ActuallyShowingPlan == null)
            {
                _ActuallyShowingPlan = plan;
                TitleText = plan.Name;
                ScrollViewerContent = plan.GenerateContentToDisplay();
                SelectedPlan = plan;
                return;
            }

            FrameHelper.NavigateToPage(typeof(View.TimetablePage),
                new TimetablePageParameter()
                {
                    ShowDefaultPage = plan == null,
                    LessonPlanToShow = plan
                });
        }

        private void RefreshTimetableButton_Click()
        {
            MainPageParameter changePageParameter = new MainPageParameter(downloadTimetable: true);
            FrameHelper.NavigateToPage(typeof(MainPage), changePageParameter);
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

        private void SetPageTexts()
        {
            MainMenuButtonText = Text.GetText(Text.TextId.TimetablePage_MainMenuButton_Text_1);
            TitleText = Text.GetText(Text.TextId.MainPage_Loading_Text_1);
            RefreshTimetableButtonText = Text.GetText(Text.TextId.TimetablePage_RefreshButton_Text_1);
            SettingsButtonText = Text.GetText(Text.TextId.TimetablePage_SettingsButton_Text_1);
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
            => SplitViewIsPaneOpen = false;

        private void PageUnloaded(object ob)
            => MVVMMessagerService.UnregisterReceiver(typeof(TimetablePageViewModel));

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
