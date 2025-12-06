var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\03\\input.txt");

var outputJoltage = 0;
foreach (var bank in lines)
{
    var firstDigit = 0;
    var position = 0;
    for (var i = 0; i < bank.Length - 1; i++)
        if (bank[i] - '0' > firstDigit)
        {
            firstDigit = bank[i] - '0';
            position = i;
        }

    var secondDigit = 0;
    for (var i = position + 1; i < bank.Length; i++)
        if (bank[i] - '0' > secondDigit)
            secondDigit = bank[i] - '0';

    outputJoltage += firstDigit * 10 + secondDigit;
}

Console.WriteLine(outputJoltage);