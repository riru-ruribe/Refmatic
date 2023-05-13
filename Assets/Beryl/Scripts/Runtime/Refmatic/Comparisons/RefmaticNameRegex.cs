using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticNameRegex : IRefmaticNameComparison
    {
        [SerializeField] RegexOptions options = RegexOptions.None;

        Regex regex;
        [NonSerialized] string lastKey = null;

        public bool IsMatch(string name, string key)
        {
            if (regex == null || lastKey != key || regex.Options != options)
            {
                regex = new Regex(key, options);
                lastKey = key;
            }
            return regex.IsMatch(name);
        }

        public RefmaticNameRegex(RegexOptions options)
        {
            this.options = options;
        }
    }
}
