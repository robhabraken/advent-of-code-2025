var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\07\\input.txt");

var splittersReached = 0;
var beams = new HashSet<int> { lines[0].IndexOf('S') };

for (var y = 2; y < lines.Length; y += 2)
    for (var x = 0; x < lines[0].Length; x++)
        if (lines[y][x].Equals('^') && beams.Contains(x))
        {
            beams.Remove(x);
            beams.Add(x - 1);
            beams.Add(x + 1);
            splittersReached++;
        }

Console.WriteLine(splittersReached);