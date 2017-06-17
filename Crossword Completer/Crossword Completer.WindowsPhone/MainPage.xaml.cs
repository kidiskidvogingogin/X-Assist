using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Phone;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Crossword_Completer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        MainViewModelWP vm;
        String viewState;
        public MainPage()
        {
            vm = new MainViewModelWP();
            this.InitializeComponent();
            this.DataContext = vm;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Definition.Width = (9 / 10.0) * Window.Current.Bounds.Width;
            viewState = ApplicationView.GetForCurrentView().Orientation.ToString();
            if (viewState == "Portrait")
            {
                North.Height = new GridLength(50.0);
                NorthSP.Orientation = Orientation.Vertical;
            }
            if (viewState == "Landscape")
            {
                North.Height = new GridLength(5.0);
                NorthSP.Orientation = Orientation.Horizontal;
            }


            //if(viewState == "Portrait")
            //{
            //    //miscInfo
            //    Grid.SetRow(miscInfo, 0);
            //    Grid.SetColumn(miscInfo, 0);
            //    Grid.SetColumnSpan(miscInfo, 2);
            //    //tboWord
            //    Grid.SetRow(tboWord, 1);
            //    Grid.SetColumn(tboWord, 0);
            //    Grid.SetColumnSpan(tboWord, 2);
            //    //lboWords
            //    Grid.SetRow(lboWords, 4);
            //    Grid.SetColumn(lboWords, 0);
            //    //wvDefinition
            //    Grid.SetRow(wvDefinition, 2);
            //    Grid.SetRowSpan(wvDefinition, 3);
            //    Grid.SetColumn(wvDefinition, 1);
            //}
            //if(viewState == "Landscape")
            //{
            //    //miscInfo
            //    Grid.SetRow(miscInfo, 2);
            //    Grid.SetColumn(miscInfo, 0);
            //    //tboWord
            //    Grid.SetRow(tboWord, 3);
            //    Grid.SetColumn(tboWord, 0);
            //    Grid.SetColumnSpan(tboWord, 1);
            //    //lboWords
            //    Grid.SetRow(lboWords, 4);
            //    Grid.SetColumn(lboWords, 0);
            //    //wvDefinition
            //    Grid.SetRow(wvDefinition, 0);
            //    Grid.SetRowSpan(wvDefinition, 5);
            //    Grid.SetColumn(wvDefinition, 1);
            //}
        }

        private void lboWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainHub.ScrollToSection(Definition);
        }

        private void lboWords_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (viewState == "Portrait")
            //{
            //    Double CombinedWidth = West.Width.Value + East.Width.Value;
            //    GridUnitType unitType = West.Width.GridUnitType;
            //    West.Width = new GridLength(1, unitType);
            //    East.Width = new GridLength(2, unitType);
            //}
        }

        private void wvDefinition_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (viewState == "Portrait")
            //{
            //    Double CombinedWidth = West.Width.Value + East.Width.Value;
            //    GridUnitType unitType = West.Width.GridUnitType;
            //    West.Width = new GridLength(1, unitType);
            //    East.Width = new GridLength(4, unitType);
            //}
        }

        private void tboWord_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (viewState == "Portrait")
            //{
            //    Double CombinedWidth = West.Width.Value + East.Width.Value;
            //    GridUnitType unitType = West.Width.GridUnitType;
            //    West.Width = new GridLength(1, unitType);
            //    East.Width = new GridLength(2, unitType);
            //}
        }
    }
}
