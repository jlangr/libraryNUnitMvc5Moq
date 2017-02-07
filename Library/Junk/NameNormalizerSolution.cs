using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Util
{
    public class NameNormalizerSolution
    {
        class Name
        {
            public Name(string fullName)
            {
                First = Last = Suffix = "";
                FullName = fullName;
                CaptureSuffix();
                SplitIntoParts();
            }

            private int CommaCount(string s)
            {
                return s.Where(c => c == ',').Count();
            }

            private bool HasSuffix(string s)
            {
                return CommaCount(s) == 1;
            }

            public void CaptureSuffix()
            {
                if (CommaCount(FullName) >= 2)
                    throw new ArgumentException("name can have at most one comma");

                if (HasSuffix(FullName))
                {
                    int suffixIndex = FullName.IndexOf(",");
                    Suffix = FullName.Substring(suffixIndex);
                    FullName = FullName.Substring(0, suffixIndex);
                }
            }

            public void SplitIntoParts()
            {
                var parts = FullName.Split(' ');
                PartsList = new LinkedList<string>(parts);
                Last = PartsList.Last.Value;
                First = PartsList.First.Value;
            }

            public bool HasOneName()
            {
                return PartsList.Count() == 1;
            }

            public string Initials
            {
                get
                {
                    var middleNames = new LinkedList<string>(PartsList);
                    middleNames.RemoveLast();
                    middleNames.RemoveFirst();
                    return string.Join(" ", middleNames.Select(name => Initial(name)));
                }
            }

            private string Initial(string name)
            {
                if (name.Count() < 2)
                    return name;
                return name[0] + ".";
            }

            public override string ToString()
            {
                if (HasOneName())
                    return First + Suffix;
                return Last + ", " + First + (Initials.Any() ? " " + Initials : "") + Suffix;
            }

            private LinkedList<string> PartsList { get; set; }
            private string FullName { get; set; }
            private string First { get; set; }
            private string Suffix { get; set; }
            private string Last { get; set; }
        }

        public string Normalize(string unnormalizedName)
        {
            if (string.IsNullOrWhiteSpace(unnormalizedName))
                return "";
            return new Name(unnormalizedName.Trim()).ToString();
        }
    }
}