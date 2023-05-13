using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class RefmaticElementLoadAttribute : Attribute, IRefmaticElementLoad
    {
        [SerializeField] string key = default;
        [SerializeField] string[] searchInFolders = default;
        [SerializeReference] IRefmaticNameComparison nameComparison = default;

        public string[] SearchInFolders => searchInFolders;

        public bool IsMatch(string name)
        {
            return nameComparison.IsMatch(name, key);
        }

        public RefmaticElementLoadAttribute(string key)
        {
            this.key = key;
            this.nameComparison = new RefmaticNameEquals();
        }
        public RefmaticElementLoadAttribute(string key, RefmaticComparisons comparison)
        {
            this.key = key;
            this.nameComparison = comparison.GetNameComparison();
        }
        public RefmaticElementLoadAttribute(string key, params string[] searchInFolders)
        {
            this.key = key;
            this.searchInFolders = searchInFolders;
            this.nameComparison = new RefmaticNameEquals();
        }
        public RefmaticElementLoadAttribute(string key, RefmaticComparisons comparison, params string[] searchInFolders)
        {
            this.key = key;
            this.searchInFolders = searchInFolders;
            this.nameComparison = comparison.GetNameComparison();
        }
        public RefmaticElementLoadAttribute(string key, RefmaticComparisons comparison, RegexOptions options, params string[] searchInFolders)
        {
            this.key = key;
            this.searchInFolders = searchInFolders;
            this.nameComparison = comparison.GetNameComparison(options);
        }
    }
}
