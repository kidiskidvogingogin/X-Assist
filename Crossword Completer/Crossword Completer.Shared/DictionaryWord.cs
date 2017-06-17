using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Crossword_Completer
{
    public class DictionaryWord : INotifyPropertyChanged
    {
        private string link;
        private bool _visible;

        public string Word { get; set; }
        public int ID { get; set; }
        public string Link 
        { 
            get{return link;}
            set
            {
                link = value;
                OnPropertyChanged("Link");
            }
        }
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public DictionaryWord()
        {
            ID = -1;
            Word = "";
            Link = "";
            Visible = true;
        }
        public DictionaryWord(int ID, string Word)
        {
            this.ID = ID;
            this.Word = Word;
            this.Link = "http://dictionary.reference.com/browse/" + Word;
            this.Visible = true;
        }
        public DictionaryWord(int ID, string Word, bool hidden)
        {
            this.ID = ID;
            this.Word = Word;
            this.Link = "http://dictionary.reference.com/browse/" + Word;
            this.Visible = !hidden;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }

            switch (name)
            {
                case "Word":
                    //LoadWords();
                    //if(SelectedItem != null)
                    //    Debug.WriteLine(SelectedItem.Link);
                    break;
                case "Visible":
                    break;
                case "Link":
                    break;
            }
        }
    }
}
