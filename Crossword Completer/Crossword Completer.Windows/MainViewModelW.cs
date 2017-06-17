using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using SQLite;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using System.Threading;

namespace Crossword_Completer
{
    public class MainViewModelW : INotifyPropertyChanged
    {
        RelayCommand _helpCommand;
        RelayCommand _addCommand;
        RelayCommand _updateCommand;
        RelayCommand _manageCommand;
        RelayCommand _contactCommand;
        RelayCommand _submitCommand;
        RelayCommand _cancelDownloadCommand;

        string word;
        string path;
        string length;
        string recordCount;
        string _newWord;
        string _progressText;
        int timerCount;
        double _progressPercent;
        bool _downloadComplete;
        DispatcherTimer inputTimer;
        DictionaryWord selectedItem;
        //DBInteraction dbI;
        CancellationTokenSource cts;
        StorageFile downloadFile;

        public string Word
        {
            get { return word; }
            set
            {
                word = value.ToLower();
                OnPropertyChanged("Word");
            }
        }
        public string Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }
        public string RecordCount
        {
            get { return recordCount; }
            set
            {
                recordCount = value;
                OnPropertyChanged("RecordCount");
            }
        }
        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                _progressText = value;
                OnPropertyChanged("ProgressText");
            }
        }
        public double ProgressPercent
        {
            get { return _progressPercent; }
            set
            {
                _progressPercent = value;
                OnPropertyChanged("ProgressPercent");
            }
        }
        public bool DownloadComplete
        {
            get { return _downloadComplete; }
            set
            {
                _downloadComplete = value;
                OnPropertyChanged("DownloadComplete");
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
        public DBInteraction DBI { get; set; }
        public string NewWord
        {
            get { return _newWord; }
            set
            {
                _newWord = value;
                OnPropertyChanged("NewWord");
            }
        }

        public ICommand HelpCommand
        {
            get
            {
                if (_helpCommand == null)
                    _helpCommand = new RelayCommand(x => HelpClicked(), x => true);
                return _helpCommand;
            }
        }
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(x => AddClicked(), x => true);
                return _addCommand;
            }
        }
        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                    _updateCommand = new RelayCommand(x => UpdateClicked(), x => true);
                return _updateCommand;
            }
        }
        public ICommand ManageCommand
        {
            get
            {
                if (_manageCommand == null)
                    _manageCommand = new RelayCommand(x => ManageClicked(), x => true);
                return _manageCommand;
            }
        }
        public ICommand ContactCommand
        {
            get
            {
                if (_contactCommand == null)
                    _contactCommand = new RelayCommand(x => ContactClicked(), x => true);
                return _contactCommand;
            }
        }
        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                    _submitCommand = new RelayCommand(x => SubmitClicked(), x => true);
                return _submitCommand;
            }
        }
        public ICommand CancelDownloadCommand
        {
            get
            {
                if (_cancelDownloadCommand == null)
                    _cancelDownloadCommand = new RelayCommand(x => CancelDownloadClicked(), x => true);
                return _cancelDownloadCommand;
            }
        }
        
        public MainViewModelW()
        {
            path = ApplicationData.Current.LocalFolder.Path + @"\WordList.sqlite";
            WordList = new ObservableCollection<DictionaryWord>();
            Length = "Leave spaces or use '_' for unknown letters";
            DBI = new DBInteraction();
            timerCount = 0;
            _progressPercent = 0;
            DownloadComplete = false;
            cts = new CancellationTokenSource();
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
                    if (Word != null)
                    {
                        inputTimer = new DispatcherTimer();
                        inputTimer.Tick += InputTimer_Tick;
                        inputTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                        timerCount++;
                        inputTimer.Start();
                        Length = "Word length: " + Word.Length;
                    }
                    break;
                case "SelectedItem":
                    break;
                case "RecordCount":
                    break;
                case "Length":
                    break;
                case "ProgressPercent":
                    ProgressText = ProgressPercent + " %";
                    break;
                case "ProgressText":
                    break;
                case "DownloadComplete":
                    break;
            }
        }

        private void InputTimer_Tick(object sender, object e)
        {
            ((DispatcherTimer)sender).Stop();
            timerCount--;
            if (timerCount == 0)
            {
                if (DBI != null)
                {
                    DBI.LoadMatchingLengthWords(WordList, Word);
                }
                RecordCount = "Number of words: " + WordList.Count;
            }
        }

        private void HelpClicked()
        {
            //Open HowTo view
            try
            {
                ((Frame)Window.Current.Content).Navigate(typeof(How_To));
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("HelpClicked() -- " + ex.Message);
            }
        }
        private void AddClicked()
        {
            if(DBI.AddWord(NewWord))
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("'" + NewWord + "' was successfully added"));

                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
            }
            NewWord = "";
            OnPropertyChanged("Word");
        }
        private async void UpdateClicked()
        {
            //begin download of new wordlist
            Uri source = new Uri("https://onedrive.live.com/download?resid=5f3a520acaad83dc!1239&authkey=!ALAne2RcYcGUXjM&ithint=file%2csqlite");
            string filename = "MasterWordList.sqlite";

            //string newDBFilePath = System.IO.Path.GetDirectoryName("Downloads");
            //System.Diagnostics.Debug.WriteLine(newDBFilePath);

            try
            {
                //downloadFile = await DownloadsFolder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
                downloadFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(source, downloadFile);

                await HandleDownloadAsync(download, true);

                //Notify user that download has completed
                ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("Download of " + download.ResultFile.Name + " completed successfully."));
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));

                //Add new words to current list
                System.Diagnostics.Debug.WriteLine(download.ResultFile.Path);
                DBI.MergeDBsAndInsert(download.ResultFile.Path);
                //dbI.MergeDBsAndInsert(@"C:\Users\wpackard\Downloads\X-Assist\MasterWordList.sqlite");

                //Cleanup Files
                if (downloadFile != null)
                    await downloadFile.DeleteAsync();

                DownloadComplete = true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateClicked() -- " + ex.Message);
            }
        }
        private void ManageClicked()
        {
            //Open Manage view
            try
            {
                //Only navigate to new window if current window size is less than 960px
                if (Window.Current.Content.RenderSize.Width < 960)
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(ManageWords), DBI);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ManageClicked() -- " + ex.Message);
            }
        }
        private async void ContactClicked()
        {
            //Open email dialogue w/ subject for contact
            Uri mailto = new Uri("mailto:?to=randyrandizzle@gmail.com&subject=Question/Comment/Snide Remark");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
        private async void SubmitClicked()
        {
            //Open email dialogue w/ subject for submit word
            Uri mailto = new Uri("mailto:?to=randyrandizzle@gmail.com&subject=Word Submission");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
        private async void CancelDownloadClicked()
        {
            if (cts != null)
            {

                if (downloadFile != null)
                    await downloadFile.DeleteAsync();
                cts.Cancel();
                cts.Dispose();
            }

            cts = new CancellationTokenSource();
            DownloadComplete = false;
            ProgressPercent = 0;
        }

        public void Dispose()
        {
            if(cts != null)
            {
                cts.Dispose();
                cts = null;
            }
        }
        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                ProgressText = "Initializing download...";
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                    await download.StartAsync().AsTask(cts.Token, progressCallback);
                else
                    await download.AttachAsync().AsTask(cts.Token, progressCallback);
            }
            catch(TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("HandleDownloadAsync() -- Task Canceled");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("HandleDownloadAsync() -- " + ex.Message);
            }
            finally
            {
                ProgressPercent = 0;
            }
        }
        private void DownloadProgress(DownloadOperation download)
        {
            if (download.Progress.TotalBytesToReceive > 0 && download.Progress.Status != BackgroundTransferStatus.Completed)
                ProgressPercent = (download.Progress.BytesReceived * 100) / (double)download.Progress.TotalBytesToReceive;
            if (download.Progress.Status == BackgroundTransferStatus.Completed || ProgressPercent >= 100)
                DownloadComplete = true;
        }

    }
}
