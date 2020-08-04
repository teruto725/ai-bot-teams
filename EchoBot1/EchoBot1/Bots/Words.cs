using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot1
{
    public class Words
    {
        private List<Word> words;
        public Words ()
        {
            words = new List<Word>();
        }
        public Boolean addTempWord(String eng,String jap)//add出来たらture
        {
            Word w = new Word(eng, jap, "temp");
            if (checkExisting(w)==false)
            {
                words.Add(w);
                return true;
            }
            return false;
        }
        public Word getWordfromEng(String eng)
        {
            return null;
        }
        public Word getWordfromJap(String jap)
        {
            return null;
        }
        public Boolean checkExisting(Word word)//trueが存在
        {
            foreach(Word wi in words)
            {
                if (word.Equals(wi))
                {
                    return true;
                }
            }
            return false;
        }
        public Boolean checkExistingJap(String jap)
        {
            if ((words.FindAll(x => x.jap == jap) == null))
            {
                return false;
            }
            return true;
        }
    }

    public class Word
    {
        public String eng;
        public String jap;
        public String state;
        public int difficulty;
        public Word(String eng, String jap,String state)
        {
            this.eng = eng;
            this.jap = jap;
            
        }
        public Boolean Equals(Word word)
        {
            if(this.eng == word.eng && this.jap == word.jap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
