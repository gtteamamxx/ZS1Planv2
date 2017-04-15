using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZS1Planv2.Model.Application;
using ZS1Planv2.Model.Plan;

namespace ZS1Planv2.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel()
        {
            UpdateTexts();
            Timetables = Plan.Instance.ClassesPlans;
            LoadSettings();
        }

        #region Command

        private RelayCommand _SaveSettingsButtonCommand;
        public RelayCommand SaveSettingsButtonCommand =>
            _SaveSettingsButtonCommand ?? (_SaveSettingsButtonCommand = new RelayCommand(() => SaveSettings()));

        #endregion

        #region Properties

        private string _StartTimetableText;
        public string StartTimetableText
        {
            get => _StartTimetableText;
            set
            {
                _StartTimetableText = value;
                OnPropertyChanged();
            }
        }

        private string _HightlightActuallyLessonsText;
        public string HightlightActuallyLessonsText
        {
            get => _HightlightActuallyLessonsText;
            set
            {
                _HightlightActuallyLessonsText = value;
                OnPropertyChanged();
            }
        }

        private string _ToogleSwitchText;
        public string ToogleSwitchText
        {
            get => _ToogleSwitchText;
            set
            {
                _ToogleSwitchText = value;
                OnPropertyChanged();
            }

        }

        private bool _ToogleSwitchIsOn;
        public bool ToogleSwitchIsOn
        {
            get => _ToogleSwitchIsOn;
            set
            {
                if (_ToogleSwitchIsOn == value)
                    return;
                _ToogleSwitchIsOn = value;

                ToogleSwitchText = Text.GetText(value
                    ? Text.TextId.SettingsUserControl_Text_3
                    : Text.TextId.SettingsUserControl_Text_4);

                OnPropertyChanged();
            }
        }

        private string _SaveText;
        public string SaveText
        {
            get => _SaveText;
            set
            {
                _SaveText = value;
                OnPropertyChanged();
            }
        }

        private List<LessonPlan> _Timetables;
        public List<LessonPlan> Timetables
        {
            get => _Timetables;
            set
            {
                if (_Timetables == value)
                    return;
                _Timetables = value;
                OnPropertyChanged();
            }
        }

        private LessonPlan _SelectedTimetable;
        public LessonPlan SelectedTimetable
        {
            get => _SelectedTimetable;
            set
            {
                if (value == _SelectedTimetable)
                    return;
                _SelectedTimetable = value;
                OnPropertyChanged();
            }
        }

        private string _SaveCompletedText;
        public string SaveCompletedText
        {
            get => _SaveCompletedText;
            set
            {
                _SaveCompletedText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void UpdateTexts()
        {
            StartTimetableText = Text.GetText(Text.TextId.SettingsUserControl_Text_1);
            HightlightActuallyLessonsText = Text.GetText(Text.TextId.SettingsUserControl_Text_2);
            SaveText = Text.GetText(Text.TextId.SettingsUserControl_Text_5);
            SaveCompletedText = Text.GetText(Text.TextId.SettingsUserControl_Text_6);
        }

        private void LoadSettings()
        {
            LessonPlan loadedPlan = Plan.Instance.ClassesPlans.FirstOrDefault(p =>
                p.Name == SettingsManager.GetSetting<string>(SettingsManager.SettingsType.Start_Plan));

            if (loadedPlan == null)
            {
                loadedPlan = Plan.Instance.ClassesPlans.First();
                SettingsManager.SetSetting(SettingsManager.SettingsType.Start_Plan, loadedPlan.Name);
            }

            SelectedTimetable = loadedPlan;
            ToogleSwitchIsOn = SettingsManager.GetSetting<bool?>(SettingsManager.SettingsType.Hightlight_Lessons) ?? false;
        }

        private void SaveSettings()
        {
            SettingsManager.SetSetting(SettingsManager.SettingsType.Start_Plan, SelectedTimetable.Name);
            SettingsManager.SetSetting(SettingsManager.SettingsType.Hightlight_Lessons, ToogleSwitchIsOn);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
