namespace MapsVisualisation.Domain.Helpers;

internal static class ArabicConverter
{
    public static int FromRoman(string roman)
    {
        Dictionary<char, int> map = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        int result = 0;

        for (int i = 0; i < roman.Length; i++)
        {
            char current = roman[i];
            char next = i < roman.Length - 1 ? roman[i + 1] : '\0';

            if (next != '\0' && map[current] < map[next])
            {
                result -= map[current];
            }
            else
            {
                result += map[current];
            }
        }

        return result;
    }
}
