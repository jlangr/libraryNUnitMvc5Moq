using System.Collections.Generic;

namespace Library.Util
{
    public class Soundex
    {
        const int MaxCodeLength = 4;

        public Soundex()
        {
            InitializeDigitLookupTable();
        }

        public string Encode(string s)
        {
            throw new NotYetImplementedException();
        }

        IDictionary<char, char> digits = new Dictionary<char, char>();

        private void InitializeDigitLookupTable()
        {
            PutAll("bfpv", '1');
            PutAll("cgjkqsxz", '2');
            PutAll("dt", '3');
            PutAll("l", '4');
            PutAll("mn", '5');
            PutAll("r", '6');
        }

        private void PutAll(string letters, char digit)
        {
            foreach (var c in letters)
                digits[c] = digit;
        }

        bool IsVowel(char letter)
        {
            return "aeiouy".IndexOf(letter) != -1;
        }

        bool IsVowelLike(char letter)
        {
            return "aeiouywh".IndexOf(letter) != -1;
        }
    }
}