using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Crossword_Completer
{
    public class WordManagementViewModelW : INotifyPropertyChanged
    {
        private string _searchWord;
        private DBInteraction dbI;
        private int timerCount;
        private DispatcherTimer inputTimer;
        private DictionaryWord _selectedWord;
        private bool _loading;
        public event PropertyChangedEventHandler PropertyChanged;

        public DictionaryWord SelectedWord
        {
            get { return _selectedWord; }
            set
            {
                _selectedWord = value;
                OnPropertyChanged("SelectedWord");
            }
        }
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                OnPropertyChanged("Loading");
            }
        }
        public ObservableCollection<DictionaryWord> WordList { get; set; }
        public string SearchWord
        {
            get { return _searchWord; }
            set
            {
                _searchWord = value;
                OnPropertyChanged("SearchWord");
            }
        }

        public WordManagementViewModelW(DBInteraction DBI)
        {
            _searchWord = "";
            dbI = DBI;
            WordList = new ObservableCollection<DictionaryWord>();
            Loading = false;
        }

        private async void InputTimer_Tick(object sender, object e)
        {
            ((DispatcherTimer)sender).Stop();
            timerCount--;
            if (timerCount == 0)
            {
                Loading = true;
                await dbI.LoadAllMatchingWordsAsync(WordList, SearchWord);
                Loading = false;
            }
        }

        void OnPropertyChanged(string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }

            switch(Name)
            {
                case "SearchWord":
                    //Start Input timer
                    //If no input for .5 seconds load list
                    inputTimer = new DispatcherTimer();
                    inputTimer.Tick += InputTimer_Tick;
                    inputTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                    timerCount++;
                    inputTimer.Start();
                    break;
                case "SelectedWord":
                    dbI.UpdateWord(SelectedWord);
                    break;
            }
        }
    }
}
