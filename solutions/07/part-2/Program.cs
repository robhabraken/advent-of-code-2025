var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\07\\input.txt");

var beams = new Dictionary<int, long> { { lines[0].IndexOf('S'), 1 } };

for (var y = 2; y < lines.Length; y += 2)
{
    var newBeams = new Dictionary<int, long>();
    foreach (var x in beams.Keys)
    {
        if (lines[y][x].Equals('^'))
        {
            set(newBeams, x - 1, beams[x]);
            set(newBeams, x + 1, beams[x]);
        }
        else
            set(newBeams, x, beams[x]);
    }
    beams = newBeams;
}

var timelines = 0L;
foreach (var timeline in beams.Keys)
    timelines += beams[timeline];

Console.WriteLine(timelines);

void set(Dictionary<int, long> target, int index, long value)
{
    if (!target.TryAdd(index, value))
        target[index] += value;
}