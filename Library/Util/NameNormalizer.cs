using System;
using System.Text;
using System.Linq;

namespace Library.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            ThrowWhenTooManyCommas(unnormalizedName);
            var trimmed = unnormalizedName.Trim();
            var trimmedWithoutSuffix = RemoveSuffix(trimmed);
            var parts = trimmedWithoutSuffix.Split(' ');

            if (parts.Length < 2) return unnormalizedName;
            return $"{LastName(parts)}, {FirstName(parts)}{MiddleInitials(parts)}{Suffix(trimmed)}";
        }

        private void ThrowWhenTooManyCommas(string unnormalizedName)
        {
            if (unnormalizedName.Count(c => c == ',') > 1)
                throw new ArgumentException("name can have at most one comma");
        }

        private string RemoveSuffix(string name)
        {
            if (name.IndexOf(',') != -1)
                return name.Split(',')[0];
            return name;
        }

        private string Suffix(string name)
        {
            if (name.IndexOf(',') != -1)
                return "," + name.Split(',')[1];
            return "";
        }

        private string MiddleInitials(string[] parts)
        {
            if (parts.Length == 2) return "";
            var middleInitials = parts.Skip(1)
                .Take(parts.Length - 2)
                .Select(p => Initial(p));
            return " " + string.Join(" ", middleInitials);
        }

        private string Initial(string name)
        {
            if (name.Length == 1) return name;
            return name[0] + ".";
        }

        private string FirstName(string[] parts)
        {
            return parts[0];
        }

        private string LastName(string[] parts)
        {
            return parts[parts.Length - 1];
        }
    }
}