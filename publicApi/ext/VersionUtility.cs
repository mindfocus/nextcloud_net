using System;
using System.Text;
using Pchp.Core;

namespace ext
{
    public static class VersionUtility
    {
        /// <summary>
        /// Compares parts of varsions delimited by '.'.
        /// </summary>
        /// <param name="part1">A part of the first version.</param>
        /// <param name="part2">A part of the second version.</param>
        /// <returns>The result of parts comparison (-1,0,+1).</returns>
        static int CompareParts(string part1, string part2)
        {
            string[] parts = { "dev", "alpha", "a", "beta", "b", "RC", " ", "#", "pl", "p" };
            int[] order = { -1, 0, 1, 1, 2, 2, 3, 4, 5, 6, 6 };

            int i = Array.IndexOf(parts, part1);
            int j = Array.IndexOf(parts, part2);
            return Math.Sign(order[i + 1] - order[j + 1]);
        }
          /// <summary>
		/// Parses a version and splits it into an array of parts.
		/// </summary>
		/// <param name="version">The version to be parsed (can be a <B>null</B> reference).</param>
		/// <returns>An array of parts.</returns>
		/// <remarks>
		/// Non-alphanumeric characters are eliminated.
		/// The version is split in between a digit following a non-digit and by   
		/// characters '.', '-', '+', '_'. 
		/// </remarks>
		static string[] VersionToArray(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return Array.Empty<string>();
            }

            var sb = new StringBuilder(version.Length);
            char last = '\0';

            for (int i = 0; i < version.Length; i++)
            {
                var ch = version[i];
                if (ch == '-' || ch == '+' || ch == '_' || ch == '.')
                {
                    if (last != '.')
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append('0'); // prepend leading '.' with '0' // TODO: test case and rewrite 'version_compare()'
                        }

                        sb.Append(last = '.');
                    }
                }
                else if (i > 0 && (char.IsDigit(ch) ^ char.IsDigit(version[i - 1])))
                {
                    if (last != '.')
                    {
                        sb.Append('.');
                    }
                    sb.Append(last = ch);
                }
                else if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(last = ch);
                }
                else
                {
                    if (last != '.')
                    {
                        sb.Append(last = '.');
                    }
                }
            }

            if (last == '.')
            {
                sb.Length--;
            }

            return sb.ToString().Split('.');
        }
        /// <summary>
        /// Compares two "PHP-standardized" version number strings.
        /// </summary>
        public static int version_compare(string version1, string version2)
        {
            string[] array1 = VersionToArray(version1);
            string[] array2 = VersionToArray(version2);
            for (int index = 0; index < Math.Max(array1.Length, array2.Length); ++index)
            {
                string str1 = index < array1.Length ? array1[index] : " ";
                string str2 = index < array2.Length ? array2[index] : " ";
                int num = !char.IsDigit(str1[0]) || !char.IsDigit(str2[0]) ? CompareParts(char.IsDigit(str1[0]) ? "#" : str1, char.IsDigit(str2[0]) ? "#" : str2) : Comparison.Compare(Pchp.Core.Convert.StringToLongInteger(str1), Pchp.Core.Convert.StringToLongInteger(str2));
                if (num != 0)
                    return num;
            }
            return 0;
        }
        /// <summary>
        /// Compares two "PHP-standardized" version number strings.
        /// </summary>
        public static bool version_compare(string version1, string version2, string op)
        {
            var compare = version_compare(version1, version2);

            switch (op)
            {
                case "<":
                case "lt": return compare < 0;

                case "<=":
                case "le": return compare <= 0;

                case ">":
                case "gt": return compare > 0;

                case ">=":
                case "ge": return compare >= 0;

                case "==":
                case "=":
                case "eq": return compare == 0;

                case "!=":
                case "<>":
                case "ne": return compare != 0;
            }

            throw new ArgumentException();  // TODO: return NULL
        }
    }
}