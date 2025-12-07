var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\07\\input.txt");

var beams = new long[lines.Length + 1, lines[0].Length];
beams[0, lines[0].IndexOf('S')] = 1;

for (var y = 0; y < lines.Length; y += 2)
    for (var x = 0; x < lines[0].Length; x++)
        if (lines[y][x].Equals('^'))
        {
            beams[y + 2, x - 1] += beams[y, x];
            beams[y + 2, x + 1] += beams[y, x];
        }
        else
            beams[y + 2, x] += beams[y, x];

var timelines = 0L;
for (var x = 0; x < lines[0].Length; x++)
    timelines += beams[lines.Length, x];

Console.WriteLine(timelines);