using System.Text.RegularExpressions;

namespace WintunerDashboard.Domain.Services;

public static class VersionComparator
{
    private static readonly Regex Tokenizer = new("[0-9]+|[a-zA-Z]+", RegexOptions.Compiled);

    public static bool IsAtLeast(string? installedVersion, string? targetVersion)
    {
        if (!TryCompare(installedVersion, targetVersion, out var result))
        {
            return false;
        }

        return result >= 0;
    }

    public static bool TryCompare(string? left, string? right, out int result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
        {
            return false;
        }

        var leftTokens = Tokenize(left);
        var rightTokens = Tokenize(right);

        var max = Math.Max(leftTokens.Count, rightTokens.Count);
        for (var i = 0; i < max; i++)
        {
            var leftToken = i < leftTokens.Count ? leftTokens[i] : null;
            var rightToken = i < rightTokens.Count ? rightTokens[i] : null;

            if (leftToken is null && rightToken is null)
            {
                break;
            }

            if (leftToken is null)
            {
                result = rightToken.IsAlpha ? 1 : -1;
                return true;
            }

            if (rightToken is null)
            {
                result = leftToken.IsAlpha ? -1 : 1;
                return true;
            }

            if (leftToken.IsNumeric && rightToken.IsNumeric)
            {
                var comparison = leftToken.Numeric.CompareTo(rightToken.Numeric);
                if (comparison != 0)
                {
                    result = comparison;
                    return true;
                }

                continue;
            }

            if (leftToken.IsAlpha && rightToken.IsAlpha)
            {
                var comparison = string.Compare(leftToken.Text, rightToken.Text, StringComparison.OrdinalIgnoreCase);
                if (comparison != 0)
                {
                    result = comparison;
                    return true;
                }

                continue;
            }

            if (leftToken.IsNumeric && rightToken.IsAlpha)
            {
                result = 1;
                return true;
            }

            if (leftToken.IsAlpha && rightToken.IsNumeric)
            {
                result = -1;
                return true;
            }
        }

        result = 0;
        return true;
    }

    private static List<VersionToken> Tokenize(string version)
    {
        var tokens = new List<VersionToken>();
        foreach (Match match in Tokenizer.Matches(version))
        {
            var value = match.Value;
            if (int.TryParse(value, out var numeric))
            {
                tokens.Add(new VersionToken(numeric, value));
            }
            else
            {
                tokens.Add(new VersionToken(value));
            }
        }

        return tokens;
    }

    private readonly record struct VersionToken
    {
        public VersionToken(int numeric, string text)
        {
            IsNumeric = true;
            Numeric = numeric;
            Text = text;
        }

        public VersionToken(string text)
        {
            IsNumeric = false;
            Numeric = 0;
            Text = text;
        }

        public bool IsNumeric { get; }
        public bool IsAlpha => !IsNumeric;
        public int Numeric { get; }
        public string Text { get; }
    }
}
