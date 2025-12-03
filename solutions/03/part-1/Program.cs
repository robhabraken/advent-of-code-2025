var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\03\\input.txt");

var joltageRating = 0;
foreach (var line in lines)
{
    var firstDigit = -1;
    var position = -1;
    for (var i = 0; i < line.Length - 1; i++)
        if (line[i] - '0' > firstDigit)
        {
            firstDigit = line[i] - '0';
            position = i;
        }

    var secondDigit = -1;
    for (var i = position + 1; i < line.Length; i++)
        if (line[i] - '0' > secondDigit)
            secondDigit = line[i] - '0';

    joltageRating += firstDigit * 10 + secondDigit;
}

Console.WriteLine(joltageRating);