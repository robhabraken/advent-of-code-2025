var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\03\\input.txt");

var outputJoltage = 0L;
foreach (var bank in lines)
    for (int startIndex = 0, digit = 11; digit >= 0; digit--)
    {
        var largestJoltageValue = 0;
        var position = 0;
        for (var i = startIndex; i < bank.Length - digit; i++)
            if (bank[i] - '0' > largestJoltageValue)
            {
                largestJoltageValue = bank[i] - '0';
                position = i;
            }

        outputJoltage += largestJoltageValue * (long)Math.Pow(10, digit);
        startIndex = position + 1;
    }

Console.WriteLine(outputJoltage);