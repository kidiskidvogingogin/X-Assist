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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Crossword_Completer
{
    public sealed partial class AddWordFlyout : UserControl
    {
        public AddWordFlyout()
        {
            this.InitializeComponent();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            this.tboAdd.Text = "";
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
