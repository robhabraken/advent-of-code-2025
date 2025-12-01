var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var password = 0;
var dial = 50;

foreach (var line in lines)
{
    var rotations = int.Parse(line[1..]);
    password += rotations / 100;
    rotations %= 100;
    if (line[0].Equals('L')) rotations *= -1;

    dial += rotations;
    if (dial < 0)
    {
        if (dial != rotations) password++;
        dial += 100;
    }
    else if (dial >= 100)
    {
        if (dial > 100) password++;
        dial -= 100;
    }

    if (dial == 0) password++;
}

Console.WriteLine(password);