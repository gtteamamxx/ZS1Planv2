using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using ZS1Planv2.Model.Extension;

namespace ZS1Planv2.Model.Application
{
    public class BackButtonHelper
    {
        private BackButtonHelper() { }

        public static void BackButtonPressed(object sender, BackRequestedEventArgs e)
        {
            Frame mainAppFrame = FrameHelper.MainFrame;

            if (IsGoBackFromPageAllowed(mainAppFrame))
            {
                mainAppFrame.GoBack();
                e.Handled = true;
                return;
            }

            e.Handled = false;
            App.Current.Exit();
        }

        private static bool IsGoBackFromPageAllowed(Frame mainAppFrame)
        {
            var currentPageType = mainAppFrame.CurrentSourcePageType;
            bool isBeforePageMainpage = mainAppFrame.BackStackDepth == 1;
            bool isBackAllowed = !isBeforePageMainpage && ((Page)mainAppFrame.Content).GetIsBackFromPageAllowed();
            return isBackAllowed;
        }
    }
}
