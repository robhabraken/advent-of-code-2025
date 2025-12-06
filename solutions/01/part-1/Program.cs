var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var dial = 50;
var password = 0;

foreach (var line in lines)
{
    var rotation = int.Parse(line[1..]);
    if (line[0].Equals('L')) rotation *= -1;

    dial += rotation;

    if (dial % 100 == 0) password++;
}

Console.WriteLine(password);