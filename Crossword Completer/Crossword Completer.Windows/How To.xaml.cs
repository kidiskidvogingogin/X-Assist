using Crossword_Completer.Common;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Crossword_Completer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class How_To : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public How_To()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            string data = 
                "<html> " +
                    "<body bgcolor=\"#252525\"> " +
                    "<font color=\"#D3D3D3\"/> ";
            //"<font color=\"" + SwitchTheme.getTheme() == "dark" ? "#D3D3D3" : "#252525" + "\"/>" +
            //if (SwitchTheme.getTheme().equals("dark"))
            //{
            //    data += "<body bgcolor=\"#252525\">";
            //    data += "<font color=\"#D3D3D3\"/>";
            //}
            //else
            //{
            //    data += "<body bgcolor=\"#FFFFFF\">";
            //    data += "<font color=\"#252525\"/>";
            //}

            data +=
                "<div align=\"center\"><h1><b>X-Assist How-To: </b></h1></div>" +
                //"</font>" +
                "<ul>" +
                    "<li>" +
                        //"<font color=\"" + SwitchTheme.getTheme() == "dark" ? "#D3D3D3" : "#252525" + "\">" +
                        "Populate the word list by typing in the indicated space at the top of the page." +
                    //"</font>" +
                    "</li> <br/>" +
                    "<li>" +

                        "Replace any spaces that you don't know the letter for with non-alphabet characters. <br/>" +
                        "(i.e. *, ., ;, 1, 2, 3, @, #, ?, %, &, etc...)" +
                    "</li> <br/>" +
                    "<li>" +
                        //"<p>" +
                        "As you type, a list will be populated that is the same length as the word that you are typing and that also has letters in the same postion that your letters are at. <br/>" +
                        "(i.e. \"?pp??\", the list of words that match is: \"appay\", \"appet\", \"apple\", \"apply\", \"eppie\", \"upper\", \"uppop\")" +
                    //"</p>" +
                    "</li> <br/>" +
                    "<li>" +
                        "Selecting a single choice in the list will bring up a definition for the selected word (if you have an active internet connection)" +
                    "</li> <br/>" +
                    "<li>" +
                        "Swiping left or right on a word will reveal a delete button. <br/>" +
                        "You have two options:" +
                            "<ul>" +
                                "<li>" +
                                    "You may swipe left or right to return the item to normal" +
                                "</li>" +
                                "<li>" +
                                    "You may select the delete icon <br/>" +
                                    "<b>THIS WILL PERMANENTLY REMOVE THE WORD FROM YOUR RESULTS UNLESS YOU MANUALLY ADD THAT WORD BACK</b>" +
                                "</li>" +
                            "</ul>" +
                    "</li> <br/>" +
                    "<li>" +
                        "The bar at the bottom of the screen allows you even more options" +
                        "<ul>" +
                            "<li>" +
                                "The sun button allows you to change themes (light on dark <--> dark on light)" +
                            "</li>" +
                            "<li>" +
                                "The \"?\" help button allows you to view this page..." +
                            "</li>" +
                            "<li>" +
                                "The \"+\" add button allows you to add new words to the list, should you wish to do so." +
                            "</li>" +
                            "<li>" +
                                "The menu button contains all other miscellaneous options" +
                            "</li>" +
                        "</ul>" +
                    "</li>" +
                "<ul>" +
            "</body>" +
        "</html>";
            wvHowTo.NavigateToString(data);
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
