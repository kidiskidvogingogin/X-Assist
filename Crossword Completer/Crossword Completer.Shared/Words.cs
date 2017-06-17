using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Crossword_Completer
{
    //[Table("Words")]
    public class Words
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(25)]
        public string word { get; set; }
        [MaxLength(500)]
        public string definition { get; set; }
        public int deleted { get; set; }
        public int hidden { get; set; }
    }
}
