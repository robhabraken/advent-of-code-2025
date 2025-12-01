var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var answer = 0;
var dial = 50;

foreach (var line in lines)
{
    var rotations = int.Parse(line[1..]) % 100;
    if (line[0].Equals('L')) rotations *= -1;

    dial += rotations;
    if (dial < 0) dial += 100;
    if (dial >= 100) dial -= 100;

    if (dial == 0) answer++;
}

Console.WriteLine(answer);