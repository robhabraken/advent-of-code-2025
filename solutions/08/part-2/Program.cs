var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\08\\input.txt");

var index = 0;
var boxes = new Dictionary<int, Junction>();
foreach (var line in lines)
    boxes.Add(index, new Junction(index++, line));

var boxesLeft = boxes.Keys.ToList<int>();

var distances = new Dictionary<string, double>();
for (var from = 0; from < boxes.Count; from++)
    for (var to = from + 1; to < boxes.Count; to++)
        distances.Add($"{boxes[from].id}-{boxes[to].id}", boxes[from].Distance(boxes[to]));

var circuits = new List<HashSet<int>>();
foreach (var pair in distances.OrderBy(key => key.Value))
{
    var junctions = pair.Key.Split('-').Select(int.Parse).ToArray();

    if (boxesLeft.Contains(junctions[0])) boxesLeft.Remove(junctions[0]);
    if (boxesLeft.Contains(junctions[1])) boxesLeft.Remove(junctions[1]);

    var occurrences = new List<int>();
    foreach (var circuit in circuits)
        if (circuit.Contains(junctions[0]) || circuit.Contains(junctions[1]))
            occurrences.Add(circuits.IndexOf(circuit));

    if (occurrences.Count == 0)
        circuits.Add(new HashSet<int>() { junctions[0], junctions[1] });
    else if (occurrences.Count == 1)
    {
        circuits[occurrences[0]].Add(junctions[0]);
        circuits[occurrences[0]].Add(junctions[1]);
    }
    else if (occurrences.Count == 2)
    {
        var target = circuits[occurrences[0]];
        foreach (var junctionBox in circuits[occurrences[1]])
            target.Add(junctionBox);
        circuits.RemoveAt(occurrences[1]);
    }

    if (circuits.Count == 1 && boxesLeft.Count == 0)
    {
        Console.WriteLine(boxes[junctions[0]].x * (long)boxes[junctions[1]].x);
        break;
    }
}

class Junction
{
    public int id;
    public int x;
    public int y;
    public int z;

    public Junction(int id, string input)
    {
        this.id = id;
        var coords = input.Split(',').Select(int.Parse).ToArray();
        x = coords[0];
        y = coords[1];
        z = coords[2];
    }

    public double Distance(Junction other)
    {
        return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2) + Math.Pow(z - other.z, 2));
    }
}