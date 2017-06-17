using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Crossword_Completer
{
    public class MainViewModelWP : INotifyPropertyChanged
    {
        RelayCommand refresh;
        String word;
        String path;
        String length;
        String recordCount;
        SQLiteConnection dbI;
        DictionaryWord selectedItem;

        public String Word
        {
            get { return word; }
            set
            {
                word = value.ToLower();
                OnPropertyChanged("Word");
            }
        }
        public String Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }
        public String RecordCount
        {
            get { return recordCount; }
            set
            {
                recordCount = value;
                OnPropertyChanged("RecordCount");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public DictionaryWord SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        public int Width { get; set; }
        public ObservableCollection<DictionaryWord> WordList { get; set; }
        public ICommand Refresh
        {
            get
            {
                if (refresh == null)
                {
                    refresh = new RelayCommand(x => this.RefreshClicked(), x => true);
                }

                return refresh;
            }
        }
        
        public MainViewModelWP()
        {
            //CopyDatabase();
            //path = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\WordList.sqlite";
            path = Package.Current.InstalledLocation.Path + @"\WordList.sqlite";
            WordList = new ObservableCollection<DictionaryWord>();
            Length = "Leave spaces or use '_' for unknown letters";
            dbI = new SQLiteConnection(path);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

            switch(name)
            {
                case "Word":
                    LoadWords();
                    RecordCount = "Number of words: " + WordList.Count;
                    Length = "Word length: " + Word.Length;
                    break;
                case "SelectedItem":
                    break;
                case "RecordCount":
                    break;
                case "Length":
                    break;
            }
        }

        private async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("WordList.sqlite");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("WordList.sqlite");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        public static IEnumerable<Words> SelectWords (SQLiteConnection db, String w)
        {
            return db.Query<Words>("SELECT ID, word, definition FROM Words WHERE LENGTH(word) = " + w.Length);
        }

        private void LoadWords()
        {
            Boolean match = false;
            WordList.Clear();
            
            //var query = dbI.Table<Words>();//.Where(x => x.word.Length == word.Length);
            //var data = query.ToList();
            foreach (Words wt in SelectWords(dbI, Word))
            {
                for (int i = 0; i < Word.Length; i++)
                {
                    if (!Char.IsLetter(Word[i]))
                        match = true;
                    else if (Word[i] == wt.word[i])
                        match = true;
                    else if (Word[i] != wt.word[i])
                        match = false;
                    if (!match)
                        break;
                }
                if (match)
                    WordList.Add(new DictionaryWord(wt.word));
            }
        }
        private void RefreshClicked()
        {
            LoadWords();
        }
    }
}
