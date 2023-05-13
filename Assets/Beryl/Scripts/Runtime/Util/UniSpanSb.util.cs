#if UNITY_2021_2_OR_NEWER
using System;
using UnityEngine;

namespace Beryl.Util
{
    public partial struct UniSpanSb : IDisposable
    {
        const string Slash = "/";
        const string Colon = ":";
        const string SemiColon = ";";
        const string Comma = ",";
        const string SquareBracket1 = "[";
        const string SquareBracket2 = "]";

        public void NameWithParent(Transform transform)
        {
            Append(transform.name);
            var t = transform.parent;
            while (t != null)
            {
                Prepend(Slash);
                Prepend(t.name);
                t = t.parent;
            }
        }
        public void NameWithParentLine(Transform transform)
        {
            NameWithParent(transform);
            AppendLine();
        }

        public void AppendSlash()
        {
            Append(Slash);
        }
        public void AppendSlash(ReadOnlySpan<char> value1, ReadOnlySpan<char> value2)
        {
            Append(value1);
            Append(Slash);
            Append(value2);
        }
        public void AppendSlash(int value1, int value2)
        {
            Append(value1);
            Append(Slash);
            Append(value2);
        }

        public void PrependColon()
        {
            Prepend(Colon);
        }
        public void AppendColon()
        {
            Append(Colon);
        }

        public void PrependSemiColon()
        {
            Prepend(SemiColon);
        }
        public void AppendSemiColon()
        {
            Append(SemiColon);
        }

        public void PrependComma()
        {
            Prepend(Comma);
        }
        public void AppendComma()
        {
            Append(Comma);
        }

        public void AppendSquareBrackets(ReadOnlySpan<char> value)
        {
            Append(SquareBracket1);
            Append(value);
            Append(SquareBracket2);
        }
        public void AppendSquareBracketsLine(ReadOnlySpan<char> value)
        {
            Append(SquareBracket1);
            Append(value);
            Append(SquareBracket2);
            AppendLine();
        }
    }
}
#endif
