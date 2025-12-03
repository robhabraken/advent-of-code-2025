var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\03\\input.txt");

var joltageRating = 0L;
foreach (var line in lines)
    for (int start = 0, digit = 11; digit >= 0; digit--)
    {
        var (value, position) = findJoltageDigit(line, start, digit);
        joltageRating += value * (long)Math.Pow(10, digit);
        start = position + 1;
    }

Console.WriteLine(joltageRating);

(int value, int position) findJoltageDigit(string bank, int startPosition, int maxPosition)
{
    var highestBatteryValue = -1;
    var highestValuePosition = -1;
    for (var i = startPosition; i < bank.Length - maxPosition; i++)
        if (bank[i] - '0' > highestBatteryValue)
        {
            highestBatteryValue = bank[i] - '0';
            highestValuePosition = i;
        }

    return (highestBatteryValue, highestValuePosition);
}