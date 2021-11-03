using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZIKS4
{
    class Wocabulary
    {
        public List<char> ch = new List<char>();
        public List<int> charCount = new List<int>();
        public void AddGrams(string s)
        {
            try
            {
                char[] chars = s.ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    int i = 0;
                    bool found = false;
                    do
                    {
                        if (ch.Count == i && !found)
                        {
                            found = true;
                            ch.Add(chars[j]);
                            charCount.Add(1);;
                        }
                        else
                        {
                            if (chars[j] == ch[i])
                            {
                                found = true;
                                charCount[i]++;
                            }
                        }
                        i++;
                    } while (!found);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public List<float> GetPossibilities()
        {
            try
            {
                List<float> Poss = new List<float>();
                int total_chars = 0;
                foreach (int i in charCount)
                    total_chars += i;
                foreach (int i in charCount)
                    Poss.Add((float)i / total_chars);
                return Poss;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InputForm());
        }
    }
}
