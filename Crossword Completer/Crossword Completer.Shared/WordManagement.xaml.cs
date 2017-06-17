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
    public sealed partial class WordManagement : UserControl
    {
        WordManagementViewModelW wmvm;
        public WordManagement()
        {
            this.InitializeComponent();
        }

        private void WordManagement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Loading":
                    if (wmvm.Loading)
                    {
                        if (LoadingPlaceholder.Visibility != Visibility.Visible)
                            LoadingPlaceholder.Visibility = Visibility.Visible;
                    }
                    else if(LoadingPlaceholder.Visibility != Visibility.Collapsed)
                    {
                        LoadingPlaceholder.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void Visibility_Toggled(object sender, RoutedEventArgs e)
        {
            ((WordManagementViewModelW)this.DataContext).SelectedWord = ((ToggleSwitch)sender).DataContext as DictionaryWord;
        }

        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            wmvm = DataContext as WordManagementViewModelW;
            wmvm.PropertyChanged += WordManagement_PropertyChanged;
        }
    }
}
