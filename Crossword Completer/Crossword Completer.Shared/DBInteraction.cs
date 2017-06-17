using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Collections.ObjectModel;

namespace Crossword_Completer
{
    public class DBInteraction
    {
        public SQLiteConnection SQL { get; set; }
        public SQLiteAsyncConnection SQLAsync { get; set; }
        public string Path { get; set; }

        public DBInteraction()
        {
            Path = ApplicationData.Current.LocalFolder.Path + @"\WordList.sqlite";
            SetConnection();
        }

        async void SetConnection()
        {
            SQL = await CopyDatabase();
            SQLAsync = new SQLiteAsyncConnection(Path);
        }
        async Task<SQLiteConnection> CopyDatabase()
        {
            bool DBExists = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("WordList.sqlite");
                DBExists = true;
            }
            catch
            {
                DBExists = false;
            }

            if (!DBExists)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("WordList.sqlite");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
            return new SQLiteConnection(Path);
        }

        public bool LoadMatchingLengthWords(ObservableCollection<DictionaryWord> Matches, string SearchTerm)
        {
            bool retVal = false;
            bool match = false;

            string query =
                "SELECT ID, word " +
                "FROM Words " +
                "WHERE LENGTH(word) = " + SearchTerm.Length +
                "   AND deleted = 0 " +
                "   AND hidden = 0 ";
            try
            {
                Matches.Clear();
                foreach (Words w in SQL.Query<Words>(query))
                {
                    for (int i = 0; i < SearchTerm.Length; i++)
                    {
                        //if (Word[i] == '_' || Word[i] == ' ')
                        if (!char.IsLetter(SearchTerm[i]))
                            match = true;
                        else if (SearchTerm[i] == w.word[i])
                            match = true;
                        else if (SearchTerm[i] != w.word[i])
                            match = false;
                        if (!match)
                            break;
                    }
                    if (match)
                        Matches.Add(new DictionaryWord(w.ID, w.word, (w.hidden == 0 ? false : true)));
                }

                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.LoadMatchingLengthWords() -- " + ex.Message);
            }

            return retVal;
        }
        public bool LoadAllMatchingWords(ObservableCollection<DictionaryWord> Matches, string SearchTerm)
        {
            bool retVal = false;

            string query =
                "SELECT ID, word, hidden " +
                "FROM Words " +
                "WHERE INSTR(word, '" + SearchTerm.Replace("'", "''") + "') > 0 " +
                "   AND LENGTH(word) >= " + SearchTerm.Length;

            try
            {
                Matches.Clear();
                foreach (Words w in SQL.Query<Words>(query))
                {
                    if(w.word.Contains(SearchTerm))
                    {
                        Matches.Add(new DictionaryWord(w.ID, w.word, (w.hidden == 0 ? false : true)));
                    }
                }

                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.LoadAllMatchingWords() -- " + ex.Message);
            }

            return retVal;
        }
        public async Task<bool> LoadAllMatchingWordsAsync(ObservableCollection<DictionaryWord> Matches, string SearchTerm)
        {
            bool retVal = false;

            string query =
                "SELECT ID, word, hidden " +
                "FROM Words " +
                "WHERE INSTR(word, '" + SearchTerm.Replace("'", "''") + "') > 0 " +
                "   AND LENGTH(word) >= " + SearchTerm.Length;

            try
            {
                Matches.Clear();
                foreach (Words w in await SQLAsync.QueryAsync<Words>(query))
                {
                    if (w.word.Contains(SearchTerm))
                    {
                        Matches.Add(new DictionaryWord(w.ID, w.word, (w.hidden == 0 ? false : true)));
                    }
                }

                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.LoadAllMatchingWords() -- " + ex.Message);
            }

            return retVal;
        }

        public bool AddWord(string word)
        {
            bool retVal = false;

            SQLiteCommand cmd;
            string query =
                "INSERT INTO Words (word, definition, deleted, hidden) " +
                "SELECT '" + word.Replace("'", "''") + "', '', 0, 0 " +
                "WHERE NOT EXISTS (SELECT word FROM Words WHERE word = '" + word.Replace("'", "''") + "') ";

            try
            {
                cmd = SQL.CreateCommand(query);
                cmd.ExecuteNonQuery();
                UpdateWord(new DictionaryWord(-1, word, false)); //if word already exists it need to be set visible
                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.AddWord() -- " + ex.Message);
            }

            return retVal;
        }
        public bool UpdateWord(DictionaryWord Word)
        {
            bool retVal = false;

            SQLiteCommand cmd;
            string query =
                "UPDATE Words " +
                "SET hidden = " + (Word.Visible ? 0 : 1) + " " +
                "WHERE ID = " + Word.ID + " " +
                "   OR word = '" + Word.Word + "' ";

            try
            {
                cmd = SQL.CreateCommand(query);
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.UpdateWord() -- " + ex.Message);
            }

            return retVal;
        }
        public bool MergeDBsAndInsert(string NewDBPath)
        {
            bool retVal = false;

            SQLiteCommand cmd;
            string Query = "";

            try
            {
                Query = "ATTACH '" + NewDBPath + "' AS Master ";
                cmd = SQL.CreateCommand(Query);
                cmd.ExecuteNonQuery();

                Query =
                    "INSERT INTO Words (word) " + 
                    "SELECT word FROM Master.Words " + 
                    "WHERE word NOT IN (SELECT word FROM Words) ";
                cmd = SQL.CreateCommand(Query);
                cmd.ExecuteNonQuery();

                Query = "DETACH 'Master' ";
                cmd = SQL.CreateCommand(Query);
                cmd.ExecuteNonQuery();
                retVal = true;
            }
            catch(Exception ex)
            {
                retVal = false;
                System.Diagnostics.Debug.WriteLine("DBInteraction.MergeDBsAndInsert() -- " + ex.Message);
            }

            return retVal;
        }
    }
}
