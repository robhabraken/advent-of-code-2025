var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\07\\input.txt");

var beams = new Dictionary<int, long>();
beams.Add(lines[0].IndexOf('S'), 1);

for (var y = 2; y < lines.Length; y += 2)
{
    var newBeams = new Dictionary<int, long>();
    foreach (var x in beams.Keys)
    {
        if (lines[y][x].Equals('^'))
        {
            if (newBeams.ContainsKey(x - 1))
                newBeams[x - 1] += beams[x];
            else
                newBeams.Add(x - 1, beams[x]);
            if (newBeams.ContainsKey(x + 1))
                newBeams[x + 1] += beams[x];
            else
                newBeams.Add(x + 1, beams[x]);
        }
        else
        {
            if (newBeams.ContainsKey(x))
                newBeams[x] += beams[x];
            else
                newBeams.Add(x, beams[x]);
        }
    }
    beams = newBeams;
}

var result = 0L;
foreach (var x in beams.Keys)
{
    result += beams[x];
}

Console.WriteLine(result);