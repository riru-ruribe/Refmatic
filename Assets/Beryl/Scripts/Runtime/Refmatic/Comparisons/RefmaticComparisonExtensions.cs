using System.Text.RegularExpressions;

namespace Beryl.Refmatic
{
    public static class RefmaticComparisonExtensions
    {
        public static IRefmaticNameComparison GetNameComparison(this RefmaticComparisons comparison, RegexOptions options = RegexOptions.None)
        {
            switch (comparison)
            {
                case RefmaticComparisons.Equals:
                    return new RefmaticNameEquals();
                case RefmaticComparisons.StartsWith:
                    return new RefmaticNameStartsWith();
                case RefmaticComparisons.EndsWith:
                    return new RefmaticNameEndsWith();
                case RefmaticComparisons.Regex:
                    return new RefmaticNameRegex(options);
            }
            return null;
        }
    }
}
