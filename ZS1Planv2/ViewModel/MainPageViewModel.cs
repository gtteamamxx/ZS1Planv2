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
using ZS1Planv2.Model.SQL;

namespace ZS1Planv2.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                    DownloadingTimetableText = Text.GetText(Text.TextId.Downloading_Text_1);
                    DownloadTimetableButtonText = Text.GetText(Text.TextId.Downloading_Text_3);
                    Blur = true;
                }
                else
                    Blur = false;

                OnPropertyChanged("DownloadTimetable");
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
                OnPropertyChanged("TimetableDownloading");
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
                OnPropertyChanged("LoadingValue");
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
                OnPropertyChanged("LoadingText");
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
                OnPropertyChanged("LoadingProgressRingActive");
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
                OnPropertyChanged("DownloadingTimetableText");
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
                OnPropertyChanged("DownloadTimetableButtonText");
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
                OnPropertyChanged("DownloadingValue");
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
                    NoInternetInfoText = Text.GetText(Text.TextId.NoInternet_Text_1);
                    NoInternetButtonText = Text.GetText(Text.TextId.NoInternet_Text_2);
                    Blur = true;
                }
                else
                    Blur = false;

                _NoInternet = value;
                OnPropertyChanged("NoInternet");
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
                OnPropertyChanged("NoInternetInfoText");
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
                OnPropertyChanged("NoInternetButtonText");
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
                OnPropertyChanged("Blur");
            }
        }
#endregion

        private void LoadTimetable()
        {

            LoadingText = Text.GetText(Text.TextId.Loading_Text_1);
            new SQLService();

            if(SQLService.Instance.LoadDatabase())
            {
                //todo
                return;
            }

            DownloadTimetable = true;
        }

        public async void DownloadTimetableButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!InternetConnection.ConnectionAvailable())
            {
                DownloadTimetable = false;
                NoInternet = true;
                return;
            }

            TimetableDownloading = true;
            DownloadingTimetableText = Text.GetText(Text.TextId.Downloading_Text_2);

            PlanDownloader planDownloader = new PlanDownloader();
            RegisterDownloadProgressEvent(planDownloader);

            Model.Plan.Plan plan = await planDownloader.DownloadNewPlanAsync();

            UnregisterDownloadProgressEvent(planDownloader);

            if(plan == null)
            {
                //todo
            }
            else
            {
                //todo
            }
        }

        private void PlanDownloader_OnDownloadProgressChanged(string name, double percentage)
        {
            DownloadingValue = percentage;
            DownloadingTimetableText = $"Trwa pobieranie... {name}";
        }

        public void NoInternetButton_Click(object sender, RoutedEventArgs e)
        {
            NoInternet = false;
            TimetableDownloading = false;
            DownloadTimetable = true;
        }

        private void RegisterDownloadProgressEvent(PlanDownloader planDownloader)
            => planDownloader.OnDownloadProgressChanged += PlanDownloader_OnDownloadProgressChanged;

        private void UnregisterDownloadProgressEvent(PlanDownloader planDownloader)
            => planDownloader.OnDownloadProgressChanged -= PlanDownloader_OnDownloadProgressChanged;

        public void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (DownloadTimetable)
                return;

            LoadTimetable();
        }

        public void PageUnloaded(object sender, RoutedEventArgs e)
            => ((MainPage)sender).DataContext = null;

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is bool))
                return;

            DownloadTimetable = ((bool)e.Parameter);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
