using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ZS1Planv2.Model.Application
{
    public class FrameHelper
    {
        private static Frame _Frame;
        public static Frame MainFrame => _Frame;

        public void SetInstance(Frame frame) => _Frame = frame;

        public static void NavigateToPage(Type type, object parameter)
            => _Frame.Navigate(type, parameter);
    }
}
