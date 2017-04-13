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
using ZS1Planv2.Model.Extension;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZS1Planv2.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimetablePage : Page
    {
        private TimetablePageViewModel _Viewmodel => this.DataContext as TimetablePageViewModel;

        public TimetablePage()
        {
            this.InitializeComponent();
            this.SetIsBackFromPageAllowed(true);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
            => _Viewmodel.OnNavigatedTo(e);
    }
}
