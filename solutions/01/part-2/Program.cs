string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\01\\input.txt");

var answer = 0;
var dial = 50;

foreach (var line in lines)
{
    var rotations = int.Parse(line[1..]);

    answer += rotations / 100; // when the number of rotations is higher than 100, for every 100 rotations we count one time passing zero
    rotations %= 100; // only rotate what's left now

    if (line[0].Equals('L'))
    {
        if (dial > 0 && rotations > dial) // if we don't leave from the dial point at zero and the number of rotations exceeds the current dial position when turning left
            answer++;

        rotations *= -1;
    }

    dial += rotations;

    if (dial < 0)
    {
        dial += 100;
    }
    else if (dial >= 100)
    {
        if (dial > 100)
            answer++; // if we go past 0 when turning right
        dial -= 100;
    }

    if (dial == 0)
        answer++; // if we end on zero
}

Console.WriteLine(answer);