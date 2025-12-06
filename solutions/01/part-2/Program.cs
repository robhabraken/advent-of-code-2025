var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var dial = 50;
var password = 0;

foreach (var line in lines)
{
    var rotation = int.Parse(line[1..]);
    password += rotation / 100;
    rotation %= 100;
    if (line[0].Equals('L')) rotation *= -1;

    dial += rotation;
    if (dial < 0)
    {
        if (dial != rotation) password++;
        dial += 100;
    }
    else if (dial > 99)
    {
        if (dial > 100) password++;
        dial -= 100;
    }

    if (dial == 0) password++;
}

Console.WriteLine(password);