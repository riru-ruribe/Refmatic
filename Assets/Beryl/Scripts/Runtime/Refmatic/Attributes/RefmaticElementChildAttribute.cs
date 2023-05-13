using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class RefmaticElementChildAttribute : Attribute, IRefmaticElementChild
    {
        [SerializeField] string key = default;
        [SerializeReference] IRefmaticNameComparison nameComparison = default;

        public bool IsMatch(string name)
        {
            return nameComparison.IsMatch(name, key);
        }

        public RefmaticElementChildAttribute(string key)
        {
            this.key = key;
            this.nameComparison = new RefmaticNameEquals();
        }
        public RefmaticElementChildAttribute(string key, RefmaticComparisons comparison)
        {
            this.key = key;
            this.nameComparison = comparison.GetNameComparison();
        }
        public RefmaticElementChildAttribute(string key, RefmaticComparisons comparison, RegexOptions options)
        {
            this.key = key;
            this.nameComparison = comparison.GetNameComparison(options);
        }
    }
}
