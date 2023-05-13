#if UNITY_2021_2_OR_NEWER
using System;
using System.Buffers;

namespace Beryl.Util
{
    public partial struct UniSpanSb : IDisposable
    {
        char[] buf;
        int idx;

        public int Length => idx;
        public ReadOnlySpan<char> AsSpan => buf.AsSpan(0, idx);
        public string Result => buf.AsSpan(0, idx).ToString();
        public string ResultWithDispose
        {
            get
            {
                var result = buf.AsSpan(0, idx).ToString();
                Dispose();
                return result;
            }
        }

        #region Prepend APIs
        public void Prepend(ReadOnlySpan<char> value)
        {
            var len = value.Length;

            Span<char> tmp = stackalloc char[idx];
            buf.AsSpan(0, idx).CopyTo(tmp);

            value.CopyTo(buf.AsSpan());
            idx += len;

            tmp.CopyTo(buf.AsSpan(len));
        }
        public void Prepend(int value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Span<char> tmp = stackalloc char[idx];
            buf.AsSpan(0, idx).CopyTo(tmp);

            if (!value.TryFormat(buf.AsSpan(), out int charsWritten, format, provider))
            {
                throw new FormatException();
            }
            idx += charsWritten;

            tmp.CopyTo(buf.AsSpan(charsWritten));
        }
        public void Prepend(float value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Span<char> tmp = stackalloc char[idx];
            buf.AsSpan(0, idx).CopyTo(tmp);

            if (!value.TryFormat(buf.AsSpan(), out int charsWritten, format, provider))
            {
                throw new FormatException();
            }
            idx += charsWritten;

            tmp.CopyTo(buf.AsSpan(charsWritten));
        }
        public void Prepend(long value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Span<char> tmp = stackalloc char[idx];
            buf.AsSpan(0, idx).CopyTo(tmp);

            if (!value.TryFormat(buf.AsSpan(), out int charsWritten, format, provider))
            {
                throw new FormatException();
            }
            idx += charsWritten;

            tmp.CopyTo(buf.AsSpan(charsWritten));
        }
        public void Prepend(Enum value)
        {
            Prepend(value.ToString());
        }
        #endregion

        #region Append APIs
        public void Append(ReadOnlySpan<char> value)
        {
            value.CopyTo(buf.AsSpan(idx));
            idx += value.Length;
        }
        public void Append(int value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            if (value.TryFormat(buf.AsSpan(idx), out int charsWritten, format, provider))
            {
                idx += charsWritten;
                return;
            }
            throw new FormatException();
        }
        public void Append(float value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            if (value.TryFormat(buf.AsSpan(idx), out int charsWritten, format, provider))
            {
                idx += charsWritten;
                return;
            }
            throw new FormatException();
        }
        public void Append(long value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            if (value.TryFormat(buf.AsSpan(idx), out int charsWritten, format, provider))
            {
                idx += charsWritten;
                return;
            }
            throw new FormatException();
        }
        public void Append(Enum value)
        {
            Append(value.ToString());
        }
        #endregion

        #region AppendLine APIs
        public void AppendLine(ReadOnlySpan<char> value)
        {
            Append(value);
            Append(Environment.NewLine);
        }
        public void AppendLine(int value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Append(value, format, provider);
            Append(Environment.NewLine);
        }
        public void AppendLine(float value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Append(value, format, provider);
            Append(Environment.NewLine);
        }
        public void AppendLine(long value, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        {
            Append(value, format, provider);
            Append(Environment.NewLine);
        }
        public void AppendLine()
        {
            Append(Environment.NewLine);
        }
        #endregion

        public void Clear()
        {
            idx = 0;
        }

        public void Dispose()
        {
            if (buf == null) return;
            ArrayPool<char>.Shared.Return(buf, clearArray: true);
            buf = null;
        }

        public UniSpanSb(int capacity)
        {
            this.buf = ArrayPool<char>.Shared.Rent(capacity);
            this.idx = 0;
        }
    }
}
#endif
