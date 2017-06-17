using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Crossword_Completer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModelW mvm;
        WordManagementViewModelW wmvm;
        public MainPage()
        {
            InitializeComponent();
            mvm = new MainViewModelW();
            DataContext = mvm;
            mvm.PropertyChanged += Mvm_PropertyChanged;

            //set management side pane
            wmvm = new WordManagementViewModelW(mvm.DBI);
            WordManagement wm = new WordManagement();
            wm.DataContext = wmvm;
            ManageControlContainer.Children.Add(wm);
            Grid.SetRow(wm, 1);
        }

        private void Mvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DownloadComplete":
                    if (mvm.DownloadComplete)
                    {
                        DownloadFlyout.Visibility = Visibility.Collapsed;
                        mvm.DownloadComplete = false;
                    }

                    break;
            }
        }

        //Show download dialogue
        private void UpdateClicked(object sender, RoutedEventArgs e)
        {
            this.DownloadFlyout.Visibility = Visibility.Visible;
            this.DownloadFlyout.Margin = new Thickness(0, 0, 0, cbOptionsBottom.ActualHeight);
        }

        //show AddWord dialogue
        private void AddClicked(object sender, RoutedEventArgs e)
        {
            this.AddFlyout.Visibility = Visibility.Visible;
            this.AddFlyout.Margin = new Thickness(0, 0, 0, cbOptionsBottom.ActualHeight);
        }

        //show ManageWord dialogue if page >= 960px
        private void ManageClicked(object sender, RoutedEventArgs e)
        {
            if(this.ActualWidth >= 960)
            {
                ManageControlContainer.Width = 400;
                wvDefinition.Width -= 400;
            }
            else
            {
                ManageControlContainer.Width = 0;
                wvDefinition.Width += 400;
            }
        }
        private void ManageCloseClicked(object sender, RoutedEventArgs e)
        {
            ManageControlContainer.Width = 0;
            wvDefinition.Width += 400;
            this.tboWord.PreventKeyboardDisplayOnProgrammaticFocus = true;
            this.tboWord.Focus(FocusState.Programmatic);
        }
        private void cbOptionsBottom_Closed(object sender, object e)
        {
            this.AddFlyout.Visibility = Visibility.Collapsed;
            this.DownloadFlyout.Visibility = Visibility.Collapsed;
            this.tboWord.PreventKeyboardDisplayOnProgrammaticFocus = true;
            this.tboWord.Focus(FocusState.Programmatic);
        }
    }
}
