#region Using Directives
using System;
using System.Linq;
using LeagueSpectator.Properties;
#endregion

namespace LeagueSpectator
{
    internal static class ExtensionMethods
    {
        #region String
        public static String[] SplitAndTrim(this String value, params String[] separators)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if ((value.Length == 0) || value.All(Char.IsWhiteSpace))
                throw new ArgumentException(Resources.ErrorStringEmpty, nameof(value));

            if (separators == null)
                throw new ArgumentNullException(nameof(separators));

            if (separators.Length == 0)
                throw new ArgumentException(Resources.ErrorStringSeparators, nameof(separators));

            String[] valueChunks = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return Array.ConvertAll(valueChunks, x => x.Trim());
        }
        #endregion
    }
}