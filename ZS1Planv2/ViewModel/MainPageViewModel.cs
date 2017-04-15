using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZS1Planv2.Model.Network;
using ZS1Planv2.Model.Others;

namespace ZS1Planv2.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        private RelayCommand _PageLoadedCommand;
        public RelayCommand PageLoadedCommand => 
            _PageLoadedCommand ?? (_PageLoadedCommand = new RelayCommand(() => PageLoadedAsync()));

        private RelayCommand _DownloadTimetableButton_ClickCommand;
        public RelayCommand DownloadTimetableButton_ClickCommand =>
            _DownloadTimetableButton_ClickCommand ?? (_DownloadTimetableButton_ClickCommand = new RelayCommand(() => DownloadTimetableButton_ClickAsync()));

        private RelayCommand _NoInternetButton_ClickCommand;
        public RelayCommand NoInternetButton_ClickCommand =>
            _NoInternetButton_ClickCommand ?? (_NoInternetButton_ClickCommand = new RelayCommand(() => NoInternetButton_Click()));

        private RelayCommand _DownloadErrorButton_ClickCommand;
        public RelayCommand DownloadErrorButton_ClickCommand =>
            _DownloadErrorButton_ClickCommand ?? (_DownloadErrorButton_ClickCommand = new RelayCommand(() => DownloadErrorButton_Click()));
#endregion

        #region Properties
        private bool _DownloadTimetable;
        public bool DownloadTimetable
        {
            get => _DownloadTimetable;
            set
            {
                if (value == _DownloadTimetable)
                    return;
                _DownloadTimetable = value;

                if (value)
                {
                    DownloadingTimetableText = Text.GetText(Text.TextId.MainPage_Downloading_Text_1);
                    DownloadTimetableButtonText = Text.GetText(Text.TextId.MainPage_Downloading_Text_3);
                    Blur = true;
                }
                else
                    Blur = false;

                OnPropertyChanged();
            }
        }

        private bool _TimetableDownloading;
        public bool TimetableDownloading
        {
            get => _TimetableDownloading;
            set
            {
                if (value == _TimetableDownloading)
                    return;
                _TimetableDownloading = value;
                OnPropertyChanged();
            }
        }

        private double _LoadingValue;
        public double LoadingValue
        {
            get => _LoadingValue;
            set
            {
                if (value == _LoadingValue)
                    return;
                _LoadingValue = value;
                OnPropertyChanged();
            }
        }

        private string _LoadingText;
        public string LoadingText
        {
            get => _LoadingText;
            set
            {
                if (value == _LoadingText)
                    return;
                _LoadingText = value;
                OnPropertyChanged();
            }
        }

        private bool _LoadingProgressRingActive;
        public bool LoadingProgressRingActive
        {
            get => _LoadingProgressRingActive;
            set
            {
                if (value == _LoadingProgressRingActive)
                    return;
                _LoadingProgressRingActive = value;
                OnPropertyChanged();
            }
        }

        private string _DownloadingTimetableText;
        public string DownloadingTimetableText
        {
            get => _DownloadingTimetableText;
            set
            {
                if (value == _DownloadingTimetableText)
                    return;
                _DownloadingTimetableText = value;
                OnPropertyChanged();
            }
        }

        private string _DownloadTimetableButtonText;
        public string DownloadTimetableButtonText
        {
            get => _DownloadTimetableButtonText;
            set
            {
                if (value == _DownloadTimetableButtonText)
                    return;
                _DownloadTimetableButtonText = value;
                OnPropertyChanged();
            }
        }

        private double _DownloadingValue;
        public double DownloadingValue
        {
            get => _DownloadingValue;
            set
            {
                if (_DownloadingValue == value)
                    return;
                _DownloadingValue = value;
                OnPropertyChanged();
            }
        }

        private bool _NoInternet;
        public bool NoInternet
        {
            get => _NoInternet;
            set
            {
                if (value == _NoInternet)
                    return;

                if (value)
                {
                    NoInternetInfoText = Text.GetText(Text.TextId.MainPage_NoInternet_Text_1);
                    NoInternetButtonText = Text.GetText(Text.TextId.MainPage_NoInternet_Text_2);
                    Blur = true;
                }
                else
                    Blur = false;

                _NoInternet = value;
                OnPropertyChanged();
            }
        }
        private string _NoInternetInfoText;
        public string NoInternetInfoText
        {
            get => _NoInternetInfoText;
            set
            {
                if (value == _NoInternetInfoText)
                    return;
                _NoInternetInfoText = value;
                OnPropertyChanged();
            }
        }

        private string _NoInternetButtonText;
        public string NoInternetButtonText
        {
            get => _NoInternetButtonText;
            set
            {
                if (value == _NoInternetButtonText)
                    return;
                _NoInternetButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _DownloadError;
        public bool DownloadError
        {
            get => _DownloadError;
            set
            {
                if (_DownloadError == value)
                    return;
                _DownloadError = value;

                if(value)
                {
                    DownloadErrorInfoText = Text.GetText(Text.TextId.MainPage_Downloading_Error_Text_1);
                    DownloadErrorButtonText = Text.GetText(Text.TextId.MainPage_Downloading_Error_Text_2);
                }

                OnPropertyChanged();
            }
        }

        private string _DownloadErrorInfoText;
        public string DownloadErrorInfoText
        {
            get => _DownloadErrorInfoText;
            set
            {
                if (_DownloadErrorInfoText == value)
                    return;
                _DownloadErrorInfoText = value;
                OnPropertyChanged();
            }
        }

        private string _DownloadErrorButtonText;
        public string DownloadErrorButtonText
        {
            get => _DownloadErrorButtonText;
            set
            {
                if (_DownloadErrorButtonText == value)
                    return;
                _DownloadErrorButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _Blur;
        public bool Blur
        {
            get => _Blur;
            set
            {
                if (value == _Blur)
                    return;
                _Blur = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private async Task LoadTimetableAsync()
        {
            LoadingText = Text.GetText(Text.TextId.MainPage_Loading_Text_1);

            Model.Plan.Plan plan = await new Model.Serializer.JSONSerializerService()
                .LoadData<Model.Plan.Plan>();
            
            if (plan != null)
            {
                plan.SetAsMainInstance();
                LoadingValue = 100;
                await Task.Delay(250);

                FrameHelper.NavigateToPage(typeof(View.TimetablePage), 
                    new Model.Others.TimetablePageParameter(showDefaultPage: true));
                return;
            }

            DownloadTimetable = true;
        }

        public async void DownloadTimetableButton_ClickAsync()
        {
            if (!InternetConnection.ConnectionAvailable())
            {
                DownloadTimetable = false;
                NoInternet = true;
                return;
            }

            TimetableDownloading = true;
            DownloadingTimetableText = Text.GetText(Text.TextId.MainPage_Downloading_Text_2);

            PlanDownloader planDownloader = new PlanDownloader();
            RegisterDownloadProgressEvent(planDownloader);

            Model.Plan.Plan plan = await planDownloader.DownloadNewPlanAsync();

            UnregisterDownloadProgressEvent(planDownloader);

            if(plan == null)
            {
                TimetableDownloading = false;
                DownloadTimetable = false;
                DownloadError = true;
            }
            else
                await SaveTimetableAsync(plan);
        }

        private async Task SaveTimetableAsync(Model.Plan.Plan plan)
        {
            DownloadingTimetableText = "Trwa zapisywanie...";
            bool result = await new Model.Serializer.JSONSerializerService().SaveData<Model.Plan.Plan>(plan);

            if (result == false)
            {
                TimetableDownloading = false;
                DownloadTimetable = false;
                DownloadError = true;
                return;
            }
            plan.SetAsMainInstance();
            await Task.Delay(250);

            FrameHelper.NavigateToPage(typeof(View.TimetablePage),
                    new Model.Others.TimetablePageParameter(showDefaultPage: true));
        }

        private void PlanDownloader_OnDownloadProgressChanged(string name, double percentage)
        {
            DownloadingValue = percentage;
            DownloadingTimetableText = $"Trwa pobieranie... {name}";
        }

        public void DownloadErrorButton_Click()
        {
            DownloadError = false;
            DownloadTimetable = true;
        }

        public void NoInternetButton_Click()
        {
            NoInternet = false;
            TimetableDownloading = false;
            DownloadTimetable = true;
        }

        private void RegisterDownloadProgressEvent(PlanDownloader planDownloader)
            => planDownloader.OnDownloadProgressChanged += PlanDownloader_OnDownloadProgressChanged;

        private void UnregisterDownloadProgressEvent(PlanDownloader planDownloader)
            => planDownloader.OnDownloadProgressChanged -= PlanDownloader_OnDownloadProgressChanged;

        public async void PageLoadedAsync()
        {
            if (DownloadTimetable)
                return;

            await LoadTimetableAsync();
        }

        public void PageUnloaded(object sender, RoutedEventArgs e)
            => ((MainPage)sender).DataContext = null;

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is MainPageParameter))
                return;

            DownloadTimetable = ((MainPageParameter)e.Parameter).DownloadTimetable;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
