using System.Globalization;

namespace Lab1;

public static class NumberFormatter
{
    public static string Format(double number)
    {
        var culture = CultureInfo.InvariantCulture;
        var formatted = number.ToString("G11", culture);

        if (formatted.Length <= 11)
        {
            return formatted;
        }

        // Switch to scientific notation and adjust exponent format
        formatted = number.ToString("E5", culture);
        var eIndex = formatted.IndexOfAny(['E', 'e']);

        if (eIndex == -1)
        {
            return formatted;
        }

        var mantissa = formatted[..eIndex];
        var exponent = formatted[(eIndex + 1)..];

        if (exponent.Length < 2)
        {
            return formatted;
        }

        var sign = exponent[0];
        var exponentDigits = exponent[1..];

        if (!int.TryParse(exponentDigits, NumberStyles.Integer, culture, out var expValue))
        {
            return formatted;
        }

        if (sign == '-' && expValue < 5)
        {
            return Math.Round(number, 9).ToString(CultureInfo.InvariantCulture);
        }

        var compactExponent = $"{sign}{expValue}";
        if (compactExponent.EndsWith("+0"))
        {
            return Math.Round(number, 9).ToString(CultureInfo.InvariantCulture);
        }

        var result = $"{mantissa}E{compactExponent}";

        return (result.Length >= 11) ? result[..11] : result;
    }
}