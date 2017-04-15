using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ZS1Planv2.Behaviors
{
    public class ListViewScrollBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            ListView listView = base.AssociatedObject;
            listView.SelectionChanged += ListView_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            ListView listView = base.AssociatedObject;
            listView.SelectionChanged -= ListView_SelectionChanged;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ListView)sender).SelectedItem;
            if (selectedItem == null)
                return;
            ((ListView)sender).ScrollIntoView(selectedItem);
        }
    }
}
