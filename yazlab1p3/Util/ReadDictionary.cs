using System.Collections.Generic;
using System.IO;

namespace yazlab1p3.Util
{
    public class ReadDictionary
    {
        public static Dictionary<string, string> Read(string path)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            StreamReader streamReader = new StreamReader(path);

            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
                string[] dizi = line.Split(';');
                var key = dizi[0];
                var value = dizi[1];

                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }
    }
}
