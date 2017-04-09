using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZS1Planv2.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZS1Planv2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel _ViewModel => this.DataContext as MainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            => _ViewModel.DownloadTimetableButton_Click(sender as Button, e);

        private void Page_Loaded(object sender, RoutedEventArgs e)
            => _ViewModel.PageLoaded(this);

        protected override void OnNavigatedTo(NavigationEventArgs e)
            => _ViewModel.OnNavigatedTo(e);

        private void Button_Click_1(object sender, RoutedEventArgs e)
            => _ViewModel.RetryDownloadTimetableButton(sender as Button, e);
    }
}
