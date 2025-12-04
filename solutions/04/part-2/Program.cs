var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\04\\input.txt");

var deltaMap = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

var rollsRemoved = 0;
var map = new bool[lines.Length, lines[0].Length];
for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
        map[y, x] = lines[y][x].Equals('@');

while (removeRolls()) ;

Console.WriteLine(rollsRemoved);

bool removeRolls()
{
    var toBeRemoved = new List<(int y, int x)>();

    for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
            if (map[y, x])
                if (isAccessible(y, x))
                    toBeRemoved.Add((y, x));

    foreach (var (y, x) in toBeRemoved)
        map[y, x] = false;

    rollsRemoved += toBeRemoved.Count;
    return toBeRemoved.Count > 0;
}

bool isAccessible(int y, int x)
{
    int dY, dX, rollsAround = 0;
    for (int i = 0; i < 8; i++)
    {
        dY = y + deltaMap[i, 0];
        dX = x + deltaMap[i, 1];

        if (dY >= 0 && dY < lines.Length && dX >= 0 && dX < lines[0].Length)
            if (map[dY, dX])
                rollsAround++;
    }
    return rollsAround < 4;
}