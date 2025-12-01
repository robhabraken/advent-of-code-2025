var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var dial = 50;
var password = 0;

foreach (var line in lines)
{
    var rotations = int.Parse(line[1..]);
    if (line[0].Equals('L')) rotations *= -1;

    dial += rotations;

    if (dial % 100 == 0) password++;
}

Console.WriteLine(password);